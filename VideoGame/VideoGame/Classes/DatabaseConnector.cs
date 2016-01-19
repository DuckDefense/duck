using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
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
        }

        public static Inventory GetInventory(int playerId) {
            Inventory inventory = new Inventory();

            if (connection.State == ConnectionState.Open) {
                GetItemLists(playerId, inventory, true);
                GetItemLists(playerId, inventory, false);
            }
            return inventory;
        }

        public static Monster GetMonster(int monsterId, int level) {
            var monsterCmd = connection.CreateCommand();
            monsterCmd.CommandText = $"SELECT * FROM `monster` WHERE `Id` = @id";
            monsterCmd.Parameters.AddWithValue("@id", monsterId);
            var monsterReader = monsterCmd.ExecuteReader();
            if (!monsterReader.HasRows) return null;
            while (monsterReader.Read()) {
                var id = monsterReader.GetInt32("Id");
                if (id == monsterId) {
                    var name = monsterReader.GetString("Name");
                    var description = monsterReader.GetString("Description");
                    var primaryType = TypeMethods.GetTypeFromString(monsterReader.GetString("PrimaryType"));
                    var secondType = TypeMethods.GetTypeFromString(monsterReader.GetString("SecondType"));
                    var maleChance = monsterReader.GetInt32("MaleChance");
                    var captureChance = monsterReader.GetInt32("CaptureChance");
                    var abilityOne = Ability.GetAbilityFromString(monsterReader.GetString("AbilityOne"));
                    var abilityTwo = Ability.GetAbilityFromString(monsterReader.GetString("AbilityTwo"));
                    var baseHealth = monsterReader.GetInt32("BaseHealth");
                    var baseAttack = monsterReader.GetInt32("BaseAttack");
                    var baseDefense = monsterReader.GetInt32("BaseDefense");
                    var baseSpecialAttack = monsterReader.GetInt32("BaseSpecialAttack");
                    var baseSpecialDefense = monsterReader.GetInt32("BaseSpecialDefense");
                    var baseSpeed = monsterReader.GetInt32("BaseSpeed");
                    var baseStats = new Stats(baseHealth, baseAttack, baseDefense, baseSpecialAttack, baseSpecialDefense, baseSpeed, level);
                    var abilities = new List<Ability>();
                    abilities.AddManyIfNotNull(abilityOne, abilityTwo);
                    var front = ContentLoader.GetTextureFromMonsterId(id, TextureFace.Front);
                    var back = ContentLoader.GetTextureFromMonsterId(id, TextureFace.Back);
                    var party = ContentLoader.GetTextureFromMonsterId(id, TextureFace.World);
                    monsterReader.Close();
                    return new Monster(id, level, name, description, primaryType, secondType, maleChance, captureChance, new Item(), baseStats, abilities, front, back, party, true);
                }
            }
            monsterReader.Close();
            return null;
        }

        private static void GetItemLists(int playerId, Inventory inventory, bool capture) {
            itemIdList.Clear();
            int amount = 0;
            var linkCmd = connection.CreateCommand();
            linkCmd.CommandText = capture ? $"SELECT * FROM `capturelink` WHERE `playerId` = @X"
                : $"SELECT * FROM `medicinelink` WHERE `playerId` = @X";
            linkCmd.Parameters.AddWithValue("@X", playerId);
            var linkReader = linkCmd.ExecuteReader();
            if (linkReader.HasRows) {
                while (linkReader.Read()) {
                    var linkId = linkReader.GetInt32(capture ? "captureId" : "medicineId");
                    amount = linkReader.GetInt32("Amount");
                    itemIdList.Add(linkId);
                }
            }
            linkReader.Close();

            foreach (var linkId in itemIdList) {
                var cmd = connection.CreateCommand();
                cmd.CommandText = capture ? $"SELECT * FROM `capture`" : $"SELECT * FROM `medicine`";

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read()) {
                        var id = reader.GetInt32("Id");
                        if (linkId == id) {
                            var cause = false;
                            var cure = Medicine.Cure.None;
                            var healAmount = 0;
                            var captureChance = 0;
                            var name = reader.GetString("Name");
                            var description = reader.GetString("Description");
                            if (capture) { captureChance = reader.GetInt32("CaptureChance"); }
                            else {
                                healAmount = reader.GetInt32("HealAmount");
                                cure = Medicine.GetCureFromString(reader.GetString("Cures"));
                                cause = reader.GetBoolean("Cause");
                            }
                            var worth = reader.GetInt32("Worth");
                            var maxAmount = reader.GetInt32("MaxAmount");

                            if (capture) inventory.Add(new Capture(id, name, description, ContentLoader.GetTextureFromCapture(id), captureChance, true, worth, amount, maxAmount), amount);
                            else inventory.Add(new Medicine(id, name, description, ContentLoader.GetTextureFromMedicine(id), healAmount, cure, worth, amount, maxAmount, cause), amount);
                        }
                    }
                reader.Close();
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
            var linkReader = linkCmd.ExecuteReader();
            if (!linkReader.HasRows) return monsterList;
            while (linkReader.Read()) {
                if (playerId == linkReader.GetInt32("playerId")) {
                    var monsterId = linkReader.GetInt32("monsterId");
                    monsterIdList.Add(monsterId);
                }
            }
            linkReader.Close();

            foreach (var monsterId in monsterIdList) {
                var item = new Item();

                linkCmd = connection.CreateCommand();
                linkCmd.CommandText = "SELECT * FROM `monsterlink` WHERE monsterId = @id";
                linkCmd.Parameters.AddWithValue("@id", monsterId);
                linkReader = linkCmd.ExecuteReader();
                if (!linkReader.HasRows) return monsterList;
                while (linkReader.Read()) {
                    Uid = linkReader.GetInt32("Uid");
                    statsId = linkReader.GetInt32("statsId");
                    level = linkReader.GetInt32("Level");
                    experience = linkReader.GetInt32("Experience");
                    itemKind = linkReader.GetString("ItemKind");
                    if (itemKind == "Capture") capture = true;
                    itemId = linkReader.GetInt32("ItemId");
                    ability = Ability.GetAbilityFromString(linkReader.GetString("Ability"));
                    gen = linkReader.GetString("Gender");
                    gender = gen == "Male" ? Gender.Male : Gender.Female;
                    boxed = linkReader.GetBoolean("Boxed");
                }
                linkReader.Close();

                var itemCmd = connection.CreateCommand();
                itemCmd.CommandText = capture
                    ? $"SELECT * FROM `capture` WHERE `Id` = {itemId}"
                    : $"SELECT * FROM `medicine` WHERE `id` = {itemId}";

                var itemReader = itemCmd.ExecuteReader();
                if (itemReader.HasRows) {
                    while (itemReader.Read()) {
                        var cause = false;
                        var cure = Medicine.Cure.None;
                        var healAmount = 0;
                        var captureChance = 0;
                        var name = itemReader.GetString("Name");
                        var description = itemReader.GetString("Description");
                        if (capture) { captureChance = itemReader.GetInt32("CaptureChance"); }
                        else {
                            healAmount = itemReader.GetInt32("HealAmount");
                            cure = Medicine.GetCureFromString(itemReader.GetString("Cures"));
                            cause = itemReader.GetBoolean("Cause");
                        }
                        var worth = itemReader.GetInt32("Worth");
                        var maxAmount = itemReader.GetInt32("MaxAmount");

                        if (capture) item = new Capture(itemId, name, description, ContentLoader.GetTextureFromCapture(itemId), captureChance, true, worth, 1, maxAmount);
                        else item = new Medicine(itemId, name, description, ContentLoader.GetTextureFromMedicine(itemId), healAmount, cure, worth, 1, maxAmount, cause);
                    }
                }
                itemReader.Close();

                var mon = GetMonster(monsterId, level);
                mon.Experience = experience;
                mon.Ability = ability;
                mon.HeldItem = item;
                mon.Gender = gender;
                mon.UId = Uid;
                mon.StatId = statsId;

                var statCmd = connection.CreateCommand();
                statCmd.CommandText = $"SELECT * FROM `stats` WHERE `Id` = {statsId}";
                var statReader = statCmd.ExecuteReader();
                if (statReader.HasRows) {
                    while (statReader.Read()) {
                        mon.Stats.Health = statReader.GetInt32("Health");
                        mon.Stats.Attack = statReader.GetInt32("Attack");
                        mon.Stats.Defense = statReader.GetInt32("Defense");
                        mon.Stats.SpecialAttack = statReader.GetInt32("SpecialAttack");
                        mon.Stats.SpecialDefense = statReader.GetInt32("SpecialDefense");
                        mon.Stats.Speed = statReader.GetInt32("Speed");
                        mon.Stats.RandAttack = statReader.GetInt32("RandAttack");
                        mon.Stats.RandDefense = statReader.GetInt32("RandDefense");
                        mon.Stats.RandSpecialAttack = statReader.GetInt32("RandSpecialAttack");
                        mon.Stats.RandSpecialDefense = statReader.GetInt32("RandSpecialDefense");
                        mon.Stats.RandSpeed = statReader.GetInt32("RandSpeed");
                    }
                }
                statReader.Close();
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
            var reader = cmd.ExecuteReader();
            if (reader.HasRows) {
                while (reader.Read()) {
                    var monsterId = reader.GetInt32("monsterId");
                    monsterIdList.Add(monsterId);
                }
            }
            reader.Close();
            foreach (var id in monsterIdList) {
                cmd.CommandText = $"SELECT `Caught` FROM `knownmonsterlink` WHERE `monsterId` = {id}";
                bool caught = (bool)cmd.ExecuteScalar();
                var mon = GetMonster(id, 0);
                if (caught) {
                    caughtMonsters.Add(id, mon);
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
            connection = new MySqlConnection(stringBuilder.ToString());
            MySqlDataReader reader;
            MySqlCommand cmd;
            connection.Open();
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
            reader = cmd.ExecuteReader();
            if (!reader.HasRows) {
                reader.Close();
                return charactersList;
            }
            while (reader.Read()) {
                var id = reader.GetInt32("Id");
                idList.Add(id);
            }
            reader.Close();

            foreach (var id in idList) {
                var inventory = GetInventory(id);
                List<Monster> box = new List<Monster>();
                var monsters = GetMonsters(id, ref box);
                var caughtMonster = new Dictionary<int, Monster>();
                var knownMonsters = GetKnownMonsters(id, ref caughtMonster);

                reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    var money = reader.GetInt32("Money");
                    var textureId = reader.GetInt32("TextureId");
                    var front = ContentLoader.GetTextureFromPlayer(textureId, TextureFace.Front);
                    var back = ContentLoader.GetTextureFromPlayer(textureId, TextureFace.Back);
                    var world = ContentLoader.GetTextureFromPlayer(textureId, TextureFace.World);
                    Vector2 position = new Vector2(reader.GetInt32("PositionX"), reader.GetInt32("PositionY"));

                    Character character = new Character(name, money, inventory, monsters, front, back,
                        world, position, true, true);
                    character.CurrentArea = new Area { Name = reader.GetString("Area") };
                    character.Id = id;
                    character.Box = box;
                    character.KnownMonsters = knownMonsters;
                    character.CaughtMonster = caughtMonster;
                    charactersList.Add(character);
                }
                reader.Close();

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
            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
                while (reader.Read()) {
                    ids.Add(reader.GetInt32("Id"));
                }
            reader.Close();
            return ids;
        }

        public static List<int> GetUids() {
            List<int> ids = new List<int>();
            var cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT `Uid` FROM `monsterlink`";
            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
                while (reader.Read()) {
                    ids.Add(reader.GetInt32("Uid"));
                }
            reader.Close();
            return ids;
        }

        public static List<int> GetLinkIds() {
            List<int> ids = new List<int>();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT `Id` FROM `monsterlink`";
            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
                while (reader.Read()) {
                    ids.Add(reader.GetInt32("Id"));
                }
            reader.Close();
            return ids;
        }

        public static List<int> GetStatsIds() {
            List<int> ids = new List<int>();
            var cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT `Id` FROM `stats`";
            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
                while (reader.Read()) {
                    ids.Add(reader.GetInt32("Id"));
                }
            reader.Close();
            return ids;
        }

        public static void SaveData(Character user) {
            //Get ids
            var playerId = user.Id;
            List<int> captureIds = user.Inventory.Captures.Select(m => m.Value.Id).ToList();
            List<int> medicineIds = user.Inventory.Medicine.Select(m => m.Value.Id).ToList();

            //Get player data
            var name = user.Name;
            var money = user.Money;
            var textureId = ContentLoader.GetIdFromCharacterWorldTexture(user.WorldSprite);
            var area = user.CurrentArea;
            var posX = user.Position.X;
            var posY = user.Position.Y;

            var cmd = connection.CreateCommand();

            #region Player

            cmd.CommandText = "SELECT COUNT(*) FROM `character` WHERE `Id` = @id";
            cmd.Parameters.AddWithValue("@id", playerId);
            var count = Convert.ToInt32(cmd.ExecuteScalar());
            cmd = connection.CreateCommand();
            if (count > 0) {
                //User is already in database
                cmd.CommandText = "UPDATE `character` SET `Money` = @money, `Area` = @area, `PositionX` = @posX, `PositionY` = @posY " +
                                  "WHERE `Id` = @id";
                cmd.Parameters.AddWithValue("@money", money);
                cmd.Parameters.AddWithValue("@area", area.Name);
                cmd.Parameters.AddWithValue("@posX", posX);
                cmd.Parameters.AddWithValue("@posY", posY);
                cmd.Parameters.AddWithValue("@id", playerId);
                cmd.ExecuteNonQuery();
            }
            else {
                cmd.CommandText = "INSERT INTO `character`(`Id`, `Name`, `Money`, `TextureId`, `Area`, `PositionX`, `PositionY`) VALUES (@pid, @name, @money, @textureId, @area, @posX, @posY)";
                cmd.Parameters.AddWithValue("@pid", playerId);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@money", money);
                cmd.Parameters.AddWithValue("@textureId", textureId);
                cmd.Parameters.AddWithValue("@area", area);
                cmd.Parameters.AddWithValue("@posX", posX);
                cmd.Parameters.AddWithValue("@posY", posY);
                cmd.ExecuteNonQuery();
            }
            #endregion
            #region Known and Caught Monsters

            var mergedList = ExtensionManager.CombineDictionaries(user.KnownMonsters, user.CaughtMonster);
            foreach (var mon in mergedList.Values) {
                cmd = connection.CreateCommand();
                cmd.CommandText =
                    "SELECT COUNT(*) FROM `knownmonsterlink` WHERE `playerId` = @playerId AND `monsterId` = @monsterId";
                cmd.Parameters.AddWithValue("@playerId", playerId);
                cmd.Parameters.AddWithValue("@monsterId", mon.Id);
                count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 0) {
                    //Player has not seen the monster before
                    cmd.CommandText = "INSERT INTO `knownmonsterlink`(`playerId`, `monsterId`, `Caught`) VALUES (@playerId, @monsterId, FALSE)";
                    cmd.ExecuteNonQuery();
                }
                if (user.CaughtMonster.ContainsKey(mon.Id)) {
                    //Player has already seen the monster and has now caught it
                    cmd.CommandText = "UPDATE `knownmonsterlink` SET `Caught` = TRUE WHERE `playerId` = @playerId AND `monsterId` = @monsterId";
                    cmd.ExecuteNonQuery();
                }
            }

            #endregion
            #region Monsters
            foreach (var mon in user.Monsters) {
                cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM `monsterlink` WHERE `Uid` = @monUid";
                cmd.Parameters.AddWithValue("@monUid", mon.UId);
                count = Convert.ToInt32(cmd.ExecuteScalar());

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
                count = Convert.ToInt32(cmd.ExecuteScalar());
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
                count = Convert.ToInt32(cmd.ExecuteScalar());
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

