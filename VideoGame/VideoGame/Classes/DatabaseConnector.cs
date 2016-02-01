using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MySql.Data.MySqlClient;
using OpenTK.Graphics.OpenGL;
using VideoGame.Classes;

namespace Sandbox.Classes {
    public static class DatabaseConnector {
        private static MySqlConnection connection;
        private static MySqlConnectionStringBuilder stringBuilder;
        private static List<int> idList = new List<int>();
        private static List<int> monsterIdList = new List<int>();
        private static List<int> itemIdList = new List<int>();

        public static void SetConnectionString(string server, string user, string password, string database) {
            stringBuilder = new MySqlConnectionStringBuilder { Server = server, UserID = user, Password = password, Database = database };
            connection = new MySqlConnection(stringBuilder.ToString());
            connection.Open();
        }

        public static Inventory GetInventory(int playerId) {
            Inventory inventory = new Inventory();

            if (connection.State == ConnectionState.Open) {
                GetItemLists(playerId, inventory, true);
                GetItemLists(playerId, inventory, false);
            }
            return inventory;
        }

        public static Monster GetMonster(int monsterId, int level)
        {
            Monster mon = null;
            var monsterCmd = connection.CreateCommand();
            monsterCmd.CommandText = $"SELECT * FROM `monster` WHERE `Id` = @id";
            monsterCmd.Parameters.AddWithValue("@id", monsterId);
            using (var rdr = monsterCmd.ExecuteReader()) {
                while (rdr.Read()) {
                    var id = rdr.GetInt32("Id");
                    if (id == monsterId) {
                        var name = rdr.GetString("Name");
                        var description = rdr.GetString("Description");
                        var primaryType = TypeMethods.GetTypeFromString(rdr.GetString("PrimaryType"));
                        var secondType = TypeMethods.GetTypeFromString(rdr.GetString("SecondType"));
                        var maleChance = rdr.GetInt32("MaleChance");
                        var captureChance = rdr.GetInt32("CaptureChance");
                        var abilityOne = Ability.GetAbilityFromString(rdr.GetString("AbilityOne"));
                        var abilityTwo = Ability.GetAbilityFromString(rdr.GetString("AbilityTwo"));
                        var baseHealth = rdr.GetInt32("BaseHealth");
                        var baseAttack = rdr.GetInt32("BaseAttack");
                        var baseDefense = rdr.GetInt32("BaseDefense");
                        var baseSpecialAttack = rdr.GetInt32("BaseSpecialAttack");
                        var baseSpecialDefense = rdr.GetInt32("BaseSpecialDefense");
                        var baseSpeed = rdr.GetInt32("BaseSpeed");
                        var baseStats = new Stats(baseHealth, baseAttack, baseDefense, baseSpecialAttack, baseSpecialDefense, baseSpeed, level);
                        var abilities = new List<Ability>();
                        abilities.AddManyIfNotNull(abilityOne, abilityTwo);
                        var front = ContentLoader.GetTextureFromMonsterId(id, TextureFace.Front);
                        var back = ContentLoader.GetTextureFromMonsterId(id, TextureFace.Back);
                        var party = ContentLoader.GetTextureFromMonsterId(id, TextureFace.World);
                        mon = new Monster(id, level, name, description, primaryType, secondType, maleChance, captureChance, new Item(), baseStats, abilities, front, back, party, true);
                        break;
                    }
                }
            }
            if(mon.Name != String.Empty) { 
                mon.UId = RandomId.GenerateRandomUId();
                mon.StatId = RandomId.GenerateStatsId();
                return mon;
            }
            return null;
        }

        private static void GetItemLists(int playerId, Inventory inventory, bool capture) {
            itemIdList.Clear();
            int amount = 0;
            var linkCmd = connection.CreateCommand();
            linkCmd.CommandText = capture ? $"SELECT * FROM `capturelink` WHERE `playerId` = @X"
                : $"SELECT * FROM `medicinelink` WHERE `playerId` = @X";
            linkCmd.Parameters.AddWithValue("@X", playerId);
            using (var rdr = linkCmd.ExecuteReader()) {
                while (rdr.Read()) {
                    var linkId = rdr.GetInt32(capture ? "captureId" : "medicineId");
                    amount = rdr.GetInt32("Amount");
                    itemIdList.Add(linkId);
                }
            }

            foreach (var linkId in itemIdList) {
                var cmd = connection.CreateCommand();
                cmd.CommandText = capture ? $"SELECT * FROM `capture`" : $"SELECT * FROM `medicine`";

                using (var rdr = cmd.ExecuteReader()) {
                    while (rdr.Read()) {
                        var id = rdr.GetInt32("Id");
                        if (linkId == id) {
                            var cause = false;
                            var cure = Medicine.Cure.None;
                            var healAmount = 0;
                            var captureChance = 0;
                            var name = rdr.GetString("Name");
                            var description = rdr.GetString("Description");
                            if (capture) { captureChance = rdr.GetInt32("CaptureChance"); }
                            else {
                                healAmount = rdr.GetInt32("HealAmount");
                                cure = Medicine.GetCureFromString(rdr.GetString("Cures"));
                                cause = rdr.GetBoolean("Cause");
                            }
                            var worth = rdr.GetInt32("Worth");
                            var maxAmount = rdr.GetInt32("MaxAmount");

                            if (capture) inventory.Add(new Capture(id, name, description, ContentLoader.GetTextureFromCapture(id), captureChance, true, worth, amount, maxAmount), amount);
                            else inventory.Add(new Medicine(id, name, description, ContentLoader.GetTextureFromMedicine(id), healAmount, cure, worth, amount, maxAmount, cause), amount);
                        }
                    }
                }
            }
        }

        public static List<Monster> GetMonsters(int playerId, ref List<Monster> box) {
            bool capture = false;
            int Uid = 0;
            int statsId = 0;
            int level = 0;
            int experience = 0;
            string itemKind = "";
            int itemId = 0;
            Ability ability = Ability.Buff();
            string gen = "";
            Gender gender = Gender.Male;
            bool boxed = false;
            var monsterList = new List<Monster>();

            var linkCmd = connection.CreateCommand();
            linkCmd.CommandText = "SELECT * FROM `monsterlink` WHERE `playerId` = @X";
            linkCmd.Parameters.AddWithValue("@X", playerId);
            using (var rdr = linkCmd.ExecuteReader()) {
                while (rdr.Read()) {
                    if (playerId == rdr.GetInt32("playerId")) {
                        var monsterId = rdr.GetInt32("monsterId");
                        monsterIdList.Add(monsterId);
                    }
                }
            }

            foreach (var monsterId in monsterIdList) {
                var item = new Item();

                linkCmd = connection.CreateCommand();
                linkCmd.CommandText = "SELECT * FROM `monsterlink` WHERE monsterId = @id";
                linkCmd.Parameters.AddWithValue("@id", monsterId);
                using (var rdr = linkCmd.ExecuteReader()) {
                    while (rdr.Read()) {
                        Uid = rdr.GetInt32("Uid");
                        statsId = rdr.GetInt32("statsId");
                        level = rdr.GetInt32("Level");
                        experience = rdr.GetInt32("Experience");
                        itemKind = rdr.GetString("ItemKind");
                        if (itemKind == "Capture") capture = true;
                        itemId = rdr.GetInt32("ItemId");
                        ability = Ability.GetAbilityFromString(rdr.GetString("Ability"));
                        gen = rdr.GetString("Gender");
                        gender = gen == "Male" ? Gender.Male : Gender.Female;
                        boxed = rdr.GetBoolean("Boxed");
                    }
                }

                var itemCmd = connection.CreateCommand();
                itemCmd.CommandText = capture
                    ? $"SELECT * FROM `capture` WHERE `Id` = {itemId}"
                    : $"SELECT * FROM `medicine` WHERE `id` = {itemId}";

                using (var rdr = itemCmd.ExecuteReader()) {
                    if (rdr.HasRows) {
                        while (rdr.Read()) {
                            var cause = false;
                            var cure = Medicine.Cure.None;
                            var healAmount = 0;
                            var captureChance = 0;
                            var name = rdr.GetString("Name");
                            var description = rdr.GetString("Description");
                            if (capture) { captureChance = rdr.GetInt32("CaptureChance"); }
                            else {
                                healAmount = rdr.GetInt32("HealAmount");
                                cure = Medicine.GetCureFromString(rdr.GetString("Cures"));
                                cause = rdr.GetBoolean("Cause");
                            }
                            var worth = rdr.GetInt32("Worth");
                            var maxAmount = rdr.GetInt32("MaxAmount");

                            if (capture) item = new Capture(itemId, name, description, ContentLoader.GetTextureFromCapture(itemId), captureChance, true, worth, 1, maxAmount);
                            else item = new Medicine(itemId, name, description, ContentLoader.GetTextureFromMedicine(itemId), healAmount, cure, worth, 1, maxAmount, cause);
                        }
                    }
                }

                var mon = GetMonster(monsterId, level);
                mon.Experience = experience;
                mon.Ability = ability;
                mon.HeldItem = item;
                mon.Gender = gender;
                mon.UId = Uid;
                mon.StatId = statsId;

                var statCmd = connection.CreateCommand();
                statCmd.CommandText = $"SELECT * FROM `stats` WHERE `Id` = {statsId}";
                using (var rdr = statCmd.ExecuteReader()) {
                    if (rdr.HasRows) {
                        while (rdr.Read()) {
                            mon.Stats.Health = rdr.GetInt32("Health");
                            mon.Stats.Attack = rdr.GetInt32("Attack");
                            mon.Stats.Defense = rdr.GetInt32("Defense");
                            mon.Stats.SpecialAttack = rdr.GetInt32("SpecialAttack");
                            mon.Stats.SpecialDefense = rdr.GetInt32("SpecialDefense");
                            mon.Stats.Speed = rdr.GetInt32("Speed");
                            mon.Stats.RandAttack = rdr.GetInt32("RandAttack");
                            mon.Stats.RandDefense = rdr.GetInt32("RandDefense");
                            mon.Stats.RandSpecialAttack = rdr.GetInt32("RandSpecialAttack");
                            mon.Stats.RandSpecialDefense = rdr.GetInt32("RandSpecialDefense");
                            mon.Stats.RandSpeed = rdr.GetInt32("RandSpeed");
                        }
                    }
                }
                if (boxed) box.Add(mon);
                else { monsterList.Add(mon); }
            }
            return monsterList;
        }

        public static Dictionary<int, Monster> GetKnownMonsters(int playerId, ref Dictionary<int, Monster> caughtMonsters) {
            var knownMonsters = new Dictionary<int, Monster>();
            monsterIdList.Clear();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM `knownmonsterlink` WHERE `playerId` = @id";
            cmd.Parameters.AddWithValue("@id", playerId);
            using (var rdr = cmd.ExecuteReader()) {
                while (rdr.Read()) {
                    var monsterId = rdr.GetInt32("monsterId");
                    monsterIdList.Add(monsterId);
                }
            }
            foreach (var id in monsterIdList) {
                cmd.CommandText = $"SELECT `Caught` FROM `knownmonsterlink` WHERE `monsterId` = {id}";
                bool caught = (bool)cmd.ExecuteScalar();
                var mon = GetMonster(id, 0);
                if (caught) {
                    caughtMonsters.Add(id, mon);
                    knownMonsters.Add(id, mon);
                }
                else {
                    knownMonsters.Add(id, mon);
                }
            }
            return knownMonsters;
        }

        /// <summary>
        /// Get all information about a character from the database
        /// </summary>
        /// <param name="name">Name of the character</param>
        public static List<Character> GetCharacters(string name) {
            //TODO: Find a way to use the id as a parameter instead of the name
            var charactersList = new List<Character>();
            MySqlDataReader reader;
            MySqlCommand cmd;
            //try {
            cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT COUNT(*) FROM `character` where `Name` = @X";
            cmd.Parameters.AddWithValue("@X", name);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count == 0) { return charactersList; }
            if (count > 1) {
                //TODO: Think of a way to handle multiple returns
                //Multiple characters found with the same name, how do we handle this?
            }
            cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM `character` WHERE `Name` = @X";
            cmd.Parameters.AddWithValue("@X", name);
            using (var rdr = cmd.ExecuteReader()) {
                while (rdr.Read()) {
                    var id = rdr.GetInt32("Id");
                    idList.Add(id);
                }
            }

            foreach (var id in idList) {
                var inventory = GetInventory(id);
                List<Monster> box = new List<Monster>();
                var monsters = GetMonsters(id, ref box);
                var caughtMonster = new Dictionary<int, Monster>();
                var knownMonsters = GetKnownMonsters(id, ref caughtMonster);

                using (var rdr = cmd.ExecuteReader()) {
                    while (rdr.Read()) {
                        var money = rdr.GetInt32("Money");
                        var textureId = rdr.GetInt32("TextureId");
                        var world = ContentLoader.GetTextureFromPlayer(textureId);
                        Vector2 position = new Vector2(rdr.GetInt32("PositionX"), rdr.GetInt32("PositionY"));

                        Character character = new Character(name, money, inventory, monsters, world, position, true, true);
                        character.CurrentArea = new Area { Name = rdr.GetString("Area") };
                        character.Id = id;
                        character.Box = box;
                        character.KnownMonsters = knownMonsters;
                        //character.CaughtMonster = caughtMonster;
                        charactersList.Add(character);
                    }
                }
            }
            foreach (var player in charactersList) {
                var areaName = player.CurrentArea.Name;
                player.CurrentArea = null;
                player.CurrentArea = Area.GetAreaFromName(areaName, player);
            }
            return charactersList;
        }
        //catch (MySqlException) {
        //    //Error with connecting
        //    return charactersList;
        //}
        //}

        public static List<int> GetPlayerIds() {
            List<int> ids = new List<int>();
            var cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT `Id` FROM `Character`";
            using (var rdr = cmd.ExecuteReader()) {
                while (rdr.Read()) {
                    ids.Add(rdr.GetInt32("Id"));
                }
            }
            return ids;
        }

        public static List<int> GetUids() {
            List<int> ids = new List<int>();
            var cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT `Uid` FROM `monsterlink`";
            using (var rdr = cmd.ExecuteReader()) {
                while (rdr.Read()) {
                    ids.Add(rdr.GetInt32("Uid"));
                }
            }
            return ids;
        }

        public static List<int> GetLinkIds() {
            List<int> ids = new List<int>();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT `Id` FROM `monsterlink`";
            using (var rdr = cmd.ExecuteReader()) {
                while (rdr.Read()) {
                    ids.Add(rdr.GetInt32("Id"));
                }
            }
            return ids;
        }

        public static List<int> GetStatsIds() {
            List<int> ids = new List<int>();
            var cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT `Id` FROM `stats`";
            using (var rdr = cmd.ExecuteReader()) {
                while (rdr.Read()) {
                    ids.Add(rdr.GetInt32("Id"));
                }
            }
            return ids;
        }

        public static int GetPlayerId(string name) {
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM `character` WHERE `name` = @name";
            cmd.Parameters.AddWithValue("@name", name);
            using (var rdr = cmd.ExecuteReader()) {
                while (rdr.Read()) {
                    var id = rdr.GetInt32("Id");
                    return id;
                }
            }
            return 0;
        }

        public static bool CheckPassword(string name, string password) {
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM `character` WHERE `Name` = @name AND `Password` = @pass";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@pass", password);
            var count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count == 1) { return true; }
            return false;
        }

        public static void AddCharacter(string name, string password, bool male) {
            var cmd = connection.CreateCommand();
            var pid = RandomId.GenerateUserId();
            cmd.CommandText = "INSERT INTO `character`(`Id`, `Name`, `Password`, `Money`, `TextureId`, `Area`, `PositionX`, `PositionY`) VALUES (@pid, @name, @pass, @money, @textureId, @area, @posX, @posY)";
            cmd.Parameters.AddWithValue("@pid", pid);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@pass", password);
            cmd.Parameters.AddWithValue("@money", 200);
            if (male) cmd.Parameters.AddWithValue("@textureId", 2);
            if (!male) cmd.Parameters.AddWithValue("@textureId", 1);
            //TODO: Make the player choose texture
            cmd.Parameters.AddWithValue("@area", "City");
            cmd.Parameters.AddWithValue("@posX", 256);
            cmd.Parameters.AddWithValue("@posY", 196);
            cmd.ExecuteNonQuery();

            //Create settings
            cmd.CommandText = "INSERT INTO `settings`(`playerId`) VALUES (@pid)";
            cmd.ExecuteNonQuery();
            SaveSettings(pid);

            //Make inventory
            cmd.CommandText = "SELECT COUNT(*) FROM `medicine`";
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            for (int i = 1; i < count; i++)
            {
                cmd.CommandText = "INSERT INTO `medicinelink`(`playerId`, `medicineId`,  `Amount`) " +
                                  $"VALUES ({pid}, {i}, 0)";
                cmd.ExecuteNonQuery();
            }
            cmd.CommandText = "SELECT COUNT(*) FROM `capture`";
            count = Convert.ToInt32(cmd.ExecuteScalar());
            for (int i = 1; i < count; i++)
            {
                cmd.CommandText = "INSERT INTO `capturelink`(`playerId`,`captureId`,`Amount`) " +
                                  $"VALUES ({pid}, {i}, 0)";
                cmd.ExecuteNonQuery();
            }
        }

        public static void GetSettings(int playerId) {
            var cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM `settings` WHERE `playerId` = @id";
            cmd.Parameters.AddWithValue("@id", playerId);
            using (var rdr = cmd.ExecuteReader()) {
                while (rdr.Read()) {
                    Enum.TryParse(rdr.GetString("moveUp"), true, out Settings.moveUp);
                    Enum.TryParse(rdr.GetString("moveDown"), true, out Settings.moveDown);
                    Enum.TryParse(rdr.GetString("moveLeft"), true, out Settings.moveLeft);
                    Enum.TryParse(rdr.GetString("moveRight"), true, out Settings.moveRight);
                    Enum.TryParse(rdr.GetString("talk"), true, out Settings.conversation);
                    Settings.ServerName = rdr.GetString("serverName");
                    Settings.Username = rdr.GetString("userId");
                    Settings.Password = rdr.GetString("userPassword");
                    Settings.DatabaseName = rdr.GetString("databaseName");
                }
            }
        }

        public static void SaveSettings(int playerId) {
            var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE `settings` SET " +
                              "`moveUp`=@moveUp, `moveDown`=@moveDown, `moveLeft`=@moveLeft, `moveRight`=@moveRight, `talk`=@talk, " +
                              "`serverName`=@serverName, `userId`=@userId, `userPassword`=@userPass, `databaseName`=@dbName " +
                              "WHERE `playerId` = @pid";
            cmd.Parameters.AddWithValue("@moveUp", Settings.moveUp.ToString());
            cmd.Parameters.AddWithValue("@moveDown", Settings.moveDown.ToString());
            cmd.Parameters.AddWithValue("@moveLeft", Settings.moveLeft.ToString());
            cmd.Parameters.AddWithValue("@moveRight", Settings.moveRight.ToString());
            cmd.Parameters.AddWithValue("@talk", Settings.conversation.ToString());
            cmd.Parameters.AddWithValue("@serverName", Settings.ServerName);
            cmd.Parameters.AddWithValue("@userId", Settings.Username);
            cmd.Parameters.AddWithValue("@userPass", Settings.Password);
            cmd.Parameters.AddWithValue("@dbName", Settings.DatabaseName);
            cmd.Parameters.AddWithValue("@pid", playerId);
            cmd.ExecuteNonQuery();
        }

        public static void SaveData(Character user) {
            //Get id
            var playerId = user.Id;

            //Get player data
            var name = user.Name;
            var money = user.Money;
            var textureId = ContentLoader.GetIdFromCharacterWorldTexture(user.WorldSprite);
            var area = user.CurrentArea;
            var posX = user.Position.X;
            var posY = user.Position.Y;
            if (user.Position.X % 32 != 0) {
                var difference = user.Position.X % 32;
                if (difference <= 16)
                    posX += difference;
                else posX -= difference;
            }
            if (user.Position.Y % 32 != 0) {
                var difference = user.Position.Y % 32;
                if (difference <= 16)
                    posY += difference;
                else posY -= difference;
            }

            var cmd = connection.CreateCommand();

            #region Player

            cmd = connection.CreateCommand();
            //User should always be in database
            cmd.CommandText = "UPDATE `character` SET `Money` = @money, `Area` = @area, `PositionX` = @posX, `PositionY` = @posY " +
                              "WHERE `Id` = @id";
            cmd.Parameters.AddWithValue("@money", money);
            cmd.Parameters.AddWithValue("@area", area.Name);
            cmd.Parameters.AddWithValue("@posX", posX);
            cmd.Parameters.AddWithValue("@posY", posY);
            cmd.Parameters.AddWithValue("@id", playerId);
            cmd.ExecuteNonQuery();

            #endregion
            #region Known and Caught Monsters

            //var mergedList = ExtensionManager.CombineDictionaries(user.KnownMonsters, user.CaughtMonster);
            foreach (var mon in user.KnownMonsters.Values) {
                cmd = connection.CreateCommand();
                cmd.CommandText =
                    "SELECT COUNT(*) FROM `knownmonsterlink` WHERE `playerId` = @playerId AND `monsterId` = @monsterId";
                cmd.Parameters.AddWithValue("@playerId", playerId);
                cmd.Parameters.AddWithValue("@monsterId", mon.Id);
                var count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 0) {
                    //Player has not seen the monster before
                    cmd.CommandText = "INSERT INTO `knownmonsterlink`(`playerId`, `monsterId`, `Caught`) VALUES (@playerId, @monsterId, FALSE)";
                    cmd.ExecuteNonQuery();
                }
            }

            #endregion
            #region Monsters
            foreach (var mon in user.Monsters) {
                cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM `monsterlink` WHERE `Uid` = @monUid";
                cmd.Parameters.AddWithValue("@monUid", mon.UId);
                var count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0) {
                    //Monster is already in database
                    //Update stats
                    cmd.CommandText =
                        "UPDATE `stats` SET `Health` = @health, `Attack` = @attack, `Defense` = @defense, `SpecialAttack` = @specAttack, `SpecialDefense` = @specDefense, `Speed` = @speed," +
                        "`RandAttack` = @randAttack, `RandDefense` = @randDefense, `RandSpecialAttack` = @randSpecAttack, `RandSpecialDefense` = @randSpecDefense, `RandSpeed` = @randSpeed " +
                        "WHERE `stats`.`Id` = @statid";
                    cmd.Parameters.AddWithValue("@health", mon.Stats.Health);
                    cmd.Parameters.AddWithValue("@attack", mon.Stats.Attack);
                    cmd.Parameters.AddWithValue("@defense", mon.Stats.Defense);
                    cmd.Parameters.AddWithValue("@specAttack", mon.Stats.SpecialAttack);
                    cmd.Parameters.AddWithValue("@specDefense", mon.Stats.SpecialDefense);
                    cmd.Parameters.AddWithValue("@speed", mon.Stats.Speed);
                    cmd.Parameters.AddWithValue("@randAttack", mon.Stats.RandAttack);
                    cmd.Parameters.AddWithValue("@randDefense", mon.Stats.RandDefense);
                    cmd.Parameters.AddWithValue("@randSpecAttack", mon.Stats.RandSpecialAttack);
                    cmd.Parameters.AddWithValue("@randSpecDefense", mon.Stats.RandSpecialDefense);
                    cmd.Parameters.AddWithValue("@randSpeed", mon.Stats.RandSpeed);
                    cmd.Parameters.AddWithValue("@statid", mon.StatId);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText =
                        "UPDATE `monsterlink` SET `monsterId` = @monId, `Level` = @level, `Experience` = @exp, `Ability` = @ability, `ItemId` = @itemId, `ItemKind` = @itemKind, `Boxed` = @box " +
                        "WHERE `Uid` = @uid";
                    cmd.Parameters.AddWithValue("@monId", mon.Id);
                    cmd.Parameters.AddWithValue("@level", mon.Level);
                    cmd.Parameters.AddWithValue("@exp", mon.Experience);
                    cmd.Parameters.AddWithValue("@ability", mon.Ability.Name);
                    cmd.Parameters.AddWithValue("@itemId", mon.HeldItem);
                    cmd.Parameters.AddWithValue("@itemKind", mon.HeldItem.Kind.ToString());
                    cmd.Parameters.AddWithValue("@box", user.Box.Contains(mon));
                    cmd.Parameters.AddWithValue("@uid", mon.UId);
                    cmd.ExecuteNonQuery();
                }
                else {
                    //Insert stats
                    cmd.CommandText =
                        "INSERT INTO `stats`(`Id`, `Health`, `Attack`, `Defense`, `SpecialAttack`, `SpecialDefense`, `Speed`, `RandAttack`, `RandDefense`, `RandSpecialAttack`, `RandSpecialDefense`, `RandSpeed`) " +
                        "VALUES(@statid, @health, @attack, @defense, @specAttack, @specDefense, @speed, @randAttack, @randDefense, @randSpecAttack, @randSpecDefense, @randSpeed)";
                    cmd.Parameters.AddWithValue("@health", mon.Stats.Health);
                    cmd.Parameters.AddWithValue("@attack", mon.Stats.Attack);
                    cmd.Parameters.AddWithValue("@defense", mon.Stats.Defense);
                    cmd.Parameters.AddWithValue("@specAttack", mon.Stats.SpecialAttack);
                    cmd.Parameters.AddWithValue("@specDefense", mon.Stats.SpecialDefense);
                    cmd.Parameters.AddWithValue("@speed", mon.Stats.Speed);
                    cmd.Parameters.AddWithValue("@randAttack", mon.Stats.RandAttack);
                    cmd.Parameters.AddWithValue("@randDefense", mon.Stats.RandDefense);
                    cmd.Parameters.AddWithValue("@randSpecAttack", mon.Stats.RandSpecialAttack);
                    cmd.Parameters.AddWithValue("@randSpecDefense", mon.Stats.RandSpecialDefense);
                    cmd.Parameters.AddWithValue("@randSpeed", mon.Stats.RandSpeed);
                    cmd.Parameters.AddWithValue("@statid", mon.StatId);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText =
                        "INSERT INTO `monsterlink`(`Id`, `Uid`, `playerId`, `monsterId`, `statsId`, `Level`, `Experience`, `Ability`, `ItemId`, `ItemKind`, `Gender`, `Boxed`) " +
                        "VALUES (@id, @uid, @pid, @mid, @sid, @level, @exp, @ability, @itemId, @itemKind, @gender, @box)";
                    var id = RandomId.GenerateLinkId();
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@uid", mon.UId);
                    cmd.Parameters.AddWithValue("@pid", playerId);
                    cmd.Parameters.AddWithValue("@mid", mon.Id);
                    cmd.Parameters.AddWithValue("@sid", mon.StatId);
                    cmd.Parameters.AddWithValue("@level", mon.Level);
                    cmd.Parameters.AddWithValue("@exp", mon.Experience);
                    cmd.Parameters.AddWithValue("@ability", mon.Ability.Name);
                    cmd.Parameters.AddWithValue("@itemId", mon.HeldItem);
                    cmd.Parameters.AddWithValue("@itemKind", mon.HeldItem.Kind.ToString());
                    cmd.Parameters.AddWithValue("@gender", mon.Gender.ToString());
                    cmd.Parameters.AddWithValue("@box", user.Box.Contains(mon));
                    cmd.ExecuteNonQuery();
                }
            }
            #endregion
            #region Inventory
            foreach (var cap in user.Inventory.Captures.Values) {
                cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM `capturelink` WHERE `playerId` = @playerId AND `captureId` = @captureId";
                cmd.Parameters.AddWithValue("@playerId", playerId);
                cmd.Parameters.AddWithValue("@captureId", cap.Id);
                var count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0) {
                    //player already has item
                    cmd.CommandText = "UPDATE `capturelink` SET `Amount` = @amount WHERE `playerId` = @playerId AND `captureId` = @captureId";
                    cmd.Parameters.AddWithValue("@amount", cap.Amount);
                    cmd.ExecuteScalar();
                }
                else {
                    //player recently receiver item
                    cmd.CommandText = "INSERT INTO `capturelink`(`playerId`, `captureId`, `Amount`) VALUES (@playerId, @captureId, @amount)";
                    cmd.Parameters.AddWithValue("@amount", cap.Amount);
                    cmd.ExecuteScalar();
                }
            }
            foreach (var med in user.Inventory.Medicine.Values) {
                cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM `medicinelink` WHERE `playerId` = @playerId AND `medicineId` = @medicineId";
                cmd.Parameters.AddWithValue("@playerId", playerId);
                cmd.Parameters.AddWithValue("@medicineId", med.Id);
                var count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0) {
                    //player already has item
                    cmd.CommandText = "UPDATE `medicinelink` SET `Amount` = @amount WHERE `playerId` = @playerId AND `medicineId` = @medicineId";
                    cmd.Parameters.AddWithValue("@amount", med.Amount);
                    cmd.ExecuteScalar();
                }
                else {
                    //player recently receiver item
                    cmd.CommandText = "INSERT INTO `medicinelink`(`playerId`, `medicineId`, `Amount`) VALUES (@playerId, @medicineId, @amount)";
                    cmd.Parameters.AddWithValue("@amount", med.Amount);
                    cmd.ExecuteScalar();
                }
            }
            #endregion
        }
    }
}

