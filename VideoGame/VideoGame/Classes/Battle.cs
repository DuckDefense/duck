﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.InputListeners;
using Sandbox.Classes.UI;

namespace VideoGame.Classes {
    public enum State {
        Battling,
        Won,
        Loss,
        Ran
    }

    public enum Selection {
        None,
        Attack,
        Item,
        Party,
        Run
    }

    public class Battle : ITimer {
        public float Interval { get; set; } = 150;
        public float Timer { get; set; } = 0;

        public Character User;
        public Character Opponent;
        public Monster CurrentUserMonster;
        public Monster CurrentOpponentMonster;
        private int boxSize, partySize;
        private int sleepTurns = 0;
        private int frozenTurns = 0;
        public int OpponentMonstersDead = 0;
        public int UserMonstersDead = 0;
        public Selection Selection = Selection.None;
        public State BattleState = State.Battling;
        public bool battleOver;
        public bool battleStart;
        public bool CountingDown = false;
        public bool playerTurn = true;
        private bool caught;
        private bool drawBattleButtons, drawMoves, drawInventory, drawItems, drawParty;
        public static Button AttackButton, RunButton, InventoryButton, PartyButton;
        public Move SelectedMove;
        public Monster SelectedMonster;
        public Medicine SelectedMedicine;
        public Capture SelectedCapture;

        private Dictionary<int, int> levelList = new Dictionary<int, int>();

        private ContainerButton prevContainer;

        /// <summary>
        /// Battle with a trainer
        /// </summary>
        public Battle(Character user, Character opponent) {
            User = user;
            Opponent = opponent;
            CurrentUserMonster = User.Monsters[0];
            CurrentOpponentMonster = Opponent.Monsters[0];
            SetupButtons();
            battleStart = true;
        }

        /// <summary>
        /// Battle with a wild monster
        /// </summary>
        public Battle(Character user, Monster opponent) {
            User = user;
            CurrentUserMonster = User.Monsters[0];
            CurrentOpponentMonster = opponent;
            SetupButtons();
            battleStart = true;
        }

        public void Attack(Monster user, Monster opponent, Move chosen) {
            var secondLine = "";
            var effective = chosen.GetDamageModifier(opponent);
            if (effective < 1) secondLine = "It's not very effective..";
            else if (effective > 1) secondLine = "It's super effective";
            //Message here saying what move the user used
            Drawer.AddMessage(new List<string> {
                            $"{user.Name} used {chosen.Name}.",
                            $"{secondLine}"
                        });
            if (chosen != null || !playerTurn) {
                chosen.Execute(user, opponent);
            }
            Reset();
        }


        public void Run(Monster user, Monster opponent) {
            int a = user.Stats.Speed;
            int b = opponent.Stats.Speed / 4;
            int c = 0;
            if (a < 4) a = 4;
            if (b < 4) b = 4;
            int f = ((a * 32) / b) + (30 * c);
            if (f < 255) {
                Random rand = new Random();
                if (rand.Next(0, 255) < f) {
                    BattleState = State.Ran;
                    battleOver = true;
                    Drawer.AddMessage(new List<string> { "You got away safely" });
                }
                else {
                    battleOver = false;
                    Drawer.AddMessage(new List<string> { "You couldn't get away" });
                    Reset(true);
                }
            }
            else {
                Drawer.AddMessage(new List<string> { "You got away safely" });
                BattleState = State.Ran;
                battleOver = true;
            }
            Selection = Selection.None;
        }

        public void Update(MouseState cur, MouseState prev, GameTime gameTime) {
            //If battle has started
            if (battleStart) {
                partySize = User.Monsters.Count;
                boxSize = User.Box.Count;
                //Store stats so the battle won't alter the stats permanently
                CurrentUserMonster.PreviousStats = CurrentUserMonster.Stats;
                CurrentOpponentMonster.PreviousStats = CurrentOpponentMonster.Stats;

                //Get Ability effects here
                CurrentUserMonster.Ability.GetStatBoosts(CurrentUserMonster);
                CurrentOpponentMonster.Ability.GetStatBoosts(CurrentOpponentMonster);
                CurrentUserMonster.Fought = true;

                foreach (var m in User.Monsters) { if (!levelList.ContainsKey(m.Id)) levelList.Add(m.Id, m.Level); }

                //Check if player has seen the monster
                if (!User.KnownMonsters.ContainsKey(CurrentOpponentMonster.Id)) User.KnownMonsters.Add(CurrentOpponentMonster.Id, CurrentOpponentMonster);
                battleStart = false;
                drawBattleButtons = true;
            }
            //If battle is happening right now.
            if (!battleOver) {
                //Choose action here, wether its an attack, using an item or switching out a monster
                if (!User.Talking) {
                    CheckDefeat();
                    UpdateButtons(cur, prev, gameTime);
                    //AilmentEffect(CurrentUserMonster);
                    //AilmentEffect(CurrentOpponentMonster);
                }
            }
            //If the battle is over
            if (battleOver) {
                drawBattleButtons = false;
                if (Opponent != null) Opponent.Defeated = true;
                //Restore the stats when the battle is over, or when the monster has been switched out
                var userHealth = CurrentUserMonster.Stats.Health;
                CurrentUserMonster.Stats = CurrentUserMonster.PreviousStats;
                CurrentUserMonster.Stats.Health = userHealth;
                CurrentOpponentMonster.Stats = CurrentOpponentMonster.PreviousStats;
                CheckState();
            }
        }

        public void Draw(SpriteBatch batch, Character player) {
            //If battle is not starting, draw everything
            if (!battleStart) {
                if (User.Talking) Drawer.DrawMessage(batch);
                Drawer.DrawBattle(batch, CurrentUserMonster, CurrentOpponentMonster);
                if (drawBattleButtons) {
                    DrawButtons(batch);
                    switch (Selection) {
                    case Selection.Attack:
                        Drawer.DrawMoves(batch, player);
                        break;
                    case Selection.Item:
                        Drawer.DrawInventory(batch, player);
                        break;
                    case Selection.Party:
                        Drawer.DrawParty(batch, player);
                        break;
                    case Selection.Run:
                        break;
                    }
                }
            }
        }

        private void CheckState() {
            switch (BattleState) {
            case State.Won:
                if (Opponent != null) {
                    User.Money += Opponent.Money;
                    //Drawer.AddMessage(new List<string> { $"{User.Name} got ${Opponent.Money} for winning" });
                }
                for (var i = 0; i < User.Monsters.Count; i++) {
                    var m = User.Monsters[i];
                    if (m.Level != 100) {
                        foreach (var levels in from levels in levelList where levels.Key == m.Id where m.Level > levels.Value where m.CanEvolve() select levels) {
                            User.Monsters[i] = m.GetEvolution();
                            var mon = User.Monsters[i];
                            if (!User.KnownMonsters.ContainsValue(mon))
                                User.KnownMonsters.Add(mon.Id, mon);
                        }
                    }
                    if (Opponent != null) Opponent.LoseMessage.Visible = true;
                }
                break;
            case State.Loss:
                if (Opponent != null) Opponent.WinMessage.Visible = true;
                Drawer.AddMessage(new List<string> { $"{User.Name} blanked out" });
                //Send the player to a healing lady
                User.CurrentArea = Area.Shop();
                User.Position = new Vector2(224, 128);
                //Heal monsters

                foreach (var m in User.Monsters) {
                    m.Stats.Health = m.MaxHealth;
                    m.Ailment = Ailment.Normal;
                    foreach (var use in m.Moves) {
                        use.Uses = use.MaxUses;
                    }
                }
                break;
            case State.Ran:
                break;
            }
        }

        private void ChangeMonster(Monster current, Character character) {
            //Store current health
            var health = current.Stats.Health;
            //Restore previous stats
            current.RestorePreviousStats();
            current.Stats = current.PreviousStats;
            current.Stats.Health = health;
            if (playerTurn) {
                var prevName = current.Name;
                character.Monsters.Move(SelectedMonster, 0);
                CurrentUserMonster = SelectedMonster;
                CurrentUserMonster.PreviousStats = CurrentUserMonster.Stats;
                CurrentUserMonster.Ability.GetStatBoosts(CurrentUserMonster);
                CurrentUserMonster.Fought = true;
                Drawer.AddMessage(new List<string> { $"You did well {prevName}! \nYou're up now {CurrentUserMonster.Name}" });
            }
            else {
                foreach (var monster in character.Monsters.Where(x => !x.IsDead)) {
                    var prevName = current.Name;
                    character.Monsters.Move(monster, 0);
                    CurrentOpponentMonster = monster;
                    CurrentOpponentMonster.PreviousStats = CurrentOpponentMonster.Stats;
                    CurrentOpponentMonster.Ability.GetStatBoosts(CurrentOpponentMonster);
                    Drawer.AddMessage(new List<string> { $"{Opponent.Name} switched out {prevName} for \n{CurrentOpponentMonster.Name}" });
                    break;
                }
                //Check if player has seen the monster
                if (!User.KnownMonsters.ContainsKey(CurrentOpponentMonster.Id)) User.KnownMonsters.Add(CurrentOpponentMonster.Id, CurrentOpponentMonster);
            }
            Selection = Selection.None;
        }

        private void CheckDefeat() {
            //If selection is run, see if the players monster is fast enough
            if (Selection == Selection.Run) Run(CurrentUserMonster, CurrentOpponentMonster);

            //If battling against an trainer
            if (Opponent != null) {
                if (!IsDefeated(User) && !IsDefeated(Opponent)) {
                    drawBattleButtons = true;
                }
                else {
                    //If they are dead the battle is over and the player either won or lost
                    if (IsDefeated(Opponent)) SetWin();
                    if (IsDefeated(User)) SetLoss();
                    battleOver = true;
                }
            }
            //If battling against a wild monster
            else {
                //If neither the user monsters or the wild monster are alive draw the buttons
                if (!IsDefeated(User) && !CurrentOpponentMonster.IsDead) {
                    drawBattleButtons = true;
                }
                //If either all user monsters are dead, or if the wild monster is dead
                else {
                    foreach (var monster in User.Monsters) {
                        if (monster.Fought)
                            if (!monster.IsDead)
                                monster.ReceiveExp(CurrentOpponentMonster);
                    }
                    if (IsDefeated(User)) SetLoss();
                    if (CurrentOpponentMonster.IsDead) SetWin();
                    battleOver = true;
                }
            }
        }

        private void SetupButtons() {
            int buttonPos = 0;

            AttackButton = new Button(new Rectangle(buttonPos, ContentLoader.GrassyBackground.Height,
                ContentLoader.Button.Width, ContentLoader.Button.Height), ContentLoader.Button, "Attack", ContentLoader.Arial);
            InventoryButton = new Button(new Rectangle((buttonPos + 64), ContentLoader.GrassyBackground.Height,
                ContentLoader.Button.Width, ContentLoader.Button.Height), ContentLoader.Button, "Items", ContentLoader.Arial);
            PartyButton = new Button(new Rectangle((buttonPos + 128), ContentLoader.GrassyBackground.Height,
                ContentLoader.Button.Width, ContentLoader.Button.Height), ContentLoader.Button, "Party", ContentLoader.Arial);
            RunButton = new Button(new Rectangle((buttonPos + 192), ContentLoader.GrassyBackground.Height,
                ContentLoader.Button.Width, ContentLoader.Button.Height), ContentLoader.Button, "Run", ContentLoader.Arial);
        }

        private void CountDown(GameTime time) {
            CountingDown = true;
            Timer += (float)time.ElapsedGameTime.TotalMilliseconds;
            if (Timer > Interval) {
                Timer = 0f;
                CountingDown = false;
            }
        }

        private void DrawButtons(SpriteBatch batch) {
            AttackButton.Draw(batch);
            InventoryButton.Draw(batch);
            PartyButton.Draw(batch);
            RunButton.Draw(batch);
        }

        private void UpdateButtons(MouseState cur, MouseState prev, GameTime time) {
            if (CountingDown) {
                CountDown(time);
            }
            else {
                if (CurrentUserMonster.IsDead) {
                    if (IsDefeated(User)) SetLoss();
                    else {
                        var container = Drawer.GetClickedContainerButton();
                        //Show the party here, and wait for the player to click a new monster
                        Selection = Selection.Party;
                        drawParty = true;
                        //Check if the clicked button is not the button that has just been pressed
                        if (prevContainer != container) {
                            for (int i = 0; i < User.Monsters.Count; i++) {
                                var m = User.Monsters[i];
                                if (m == container.Monster) {
                                    SelectedMonster = m;
                                    ChangeMonster(CurrentUserMonster, User);
                                    Reset(false);
                                    playerTurn = true;
                                }
                            }
                        }
                    }
                }
                if (CurrentOpponentMonster.IsDead) {
                    foreach (var monster in User.Monsters) {
                        if (monster.Fought)
                            if (!monster.IsDead)
                                monster.ReceiveExp(CurrentOpponentMonster);
                    }
                    if (IsDefeated(Opponent))
                        SetWin();
                    else {
                        ChangeMonster(CurrentOpponentMonster, Opponent);
                    }
                }
                else {
                    if (playerTurn) {
                        if (!CurrentUserMonster.Ability.SkipTurn(CurrentUserMonster)
                            && CurrentUserMonster.Ailment != Ailment.Frozen
                            && CurrentUserMonster.Ailment != Ailment.Sleep) {
                            AttackButton.Update(cur, prev);
                            InventoryButton.Update(cur, prev);
                            PartyButton.Update(cur, prev);
                            RunButton.Update(cur, prev);

                            if (AttackButton.IsClicked(cur, prev)) {
                                Selection = Selection.Attack;
                                ResetDraws();
                                drawMoves = true;
                            }
                            else if (InventoryButton.IsClicked(cur, prev)) {
                                Selection = Selection.Item;
                                ResetDraws();
                                drawInventory = true;
                            }
                            else if (PartyButton.IsClicked(cur, prev)) {
                                Selection = Selection.Party;
                                ResetDraws();
                                drawParty = true;
                            }
                            else if (RunButton.IsClicked(cur, prev)) {
                                Selection = Selection.Run;
                                ResetDraws();
                            }
                            GetSelected(cur);
                        }
                        else {
                            playerTurn = false;
                            if (CurrentUserMonster.Ailment == Ailment.Frozen)
                                Drawer.AddMessage(new List<string>
                                   {"Is frozen solid"});
                            else if (CurrentUserMonster.Ailment == Ailment.Frozen)
                                Drawer.AddMessage(new List<string>
                                {"Is Fast asleep"});
                        }
                    }
                    else {
                        if (!CurrentOpponentMonster.Ability.SkipTurn(CurrentOpponentMonster)) {
                            //Add a little delay
                            CountDown(time);
                            //Reset all choices
                            ResetDraws();
                            Selection = Selection.None;
                            BattleAI.MakeDecision(this, SelectedMove, CurrentOpponentMonster, CurrentUserMonster,
                                Opponent);
                        }
                        playerTurn = true;
                    }
                }
            }
            prevContainer = Drawer.GetClickedContainerButton();
        }

        private void GetSelected(MouseState cur) {
            var container = Drawer.GetClickedContainerButton();

            if (container != null && container != prevContainer && container.Button.IsHeld(cur)) {
                //Get selected move
                if (drawMoves) {
                    foreach (var m in CurrentUserMonster.Moves.Where(x => x.Name == container.Move.Name).Where(m => m.Uses != 0)) {
                        SelectedMove = m;
                        break;
                    }
                    if (SelectedMove != null)
                        if (SelectedMove.Uses != 0) {
                            Attack(CurrentUserMonster, CurrentOpponentMonster, SelectedMove);
                            playerTurn = false;
                        }
                        else {
                            Drawer.AddMessage(new List<string> { "This use is all out of uses" });
                            playerTurn = true;
                        }
                }
                //Get selected monster from party
                if (drawParty) {
                    foreach (var m in User.Monsters.Where(m => m == container.Monster)) {
                        SelectedMonster = m;
                        ChangeMonster(CurrentUserMonster, User);
                        Reset();
                        break;
                    }
                }
                if (Drawer.DrawMedicine) {
                    foreach (var m in User.Inventory.Medicine.Where(x => x.Value == container.Medicine))
                        SelectedMedicine = m.Value;
                    if (SelectedMedicine != null) {
                        Drawer.AddMessage(new List<string> { $"{SelectedMedicine.Name} used on {SelectedMonster.Name}." });

                        SelectedMedicine.Use(CurrentUserMonster, User);
                    }
                    Reset();
                }
                if (Drawer.DrawCapture) {
                    foreach (var m in User.Inventory.Captures.Where(x => x.Value == container.Capture))
                        SelectedCapture = m.Value;
                    if (SelectedCapture != null) {
                        Drawer.AddMessage(new List<string> { $"{SelectedCapture.Name} used on {CurrentOpponentMonster.Name}." });

                        SelectedCapture.Use(CurrentOpponentMonster, User);
                    }
                    //Check if the monster has been caught
                    if (User.Monsters.Count == partySize) {
                        if (User.Box.Count != boxSize) {
                            Drawer.AddMessage(new List<string> { $"Heck yes! You caught {CurrentOpponentMonster.Name}!" });
                            SetWin();
                        }
                    }
                    else {
                        Drawer.AddMessage(new List<string> { $"Heck yes! You caught {CurrentOpponentMonster.Name}!" });
                        SetWin();
                    }
                    Reset();
                }
            }
            prevContainer = Drawer.GetClickedContainerButton();
        }

        private void ResetDraws() {
            drawMoves = false;
            drawInventory = false;
            drawParty = false;
            Drawer.DrawCapture = false;
            Drawer.DrawMedicine = false;
        }
        private void Reset(bool passTurn = true) {
            ResetDraws();
            AilmentEffect(CurrentUserMonster);
            //AilmentEffect(CurrentOpponentMonster);
            Selection = Selection.None;
            SelectedMedicine = null;
            SelectedCapture = null;
            SelectedMonster = null;
            SelectedMove = null;
            if (passTurn) playerTurn = false;
        }
        private bool IsDefeated(Character character) {
            if (Opponent == null) if (CurrentOpponentMonster.IsDead) return true;
            var deadMonsters = character.Monsters.Count(m => m.IsDead);
            //Check if any monsters of the opponent are dead
            return deadMonsters == character.Monsters.Count;
        }
        private void SetWin() {
            BattleState = State.Won;
            battleOver = true;
        }
        private void SetLoss() {
            BattleState = State.Loss;
            battleOver = true;
        }
        public void AilmentEffect(Monster monster) {
            if (monster.Ailment == Ailment.Burned) {
                monster.Stats.Health -= (monster.MaxHealth / 100 * 8);
            }
            if (monster.Ailment == Ailment.Poisoned) {
                monster.Stats.Health -= Convert.ToInt32(((double)monster.MaxHealth / 100) * 8);
            }
            if (monster.Ailment == Ailment.Sleep) {
                if (sleepTurns == 3) {
                    monster.Ailment = Ailment.Normal;
                    sleepTurns = 0;
                }
                else {
                    Random r = new Random();
                    if (r.Next(0, 100) < 40) {
                        monster.Ailment = Ailment.Normal;
                        sleepTurns = 0;
                    }
                    else {
                        sleepTurns++;
                    }
                }
            }
            if (monster.Ailment == Ailment.Frozen) {
                if (frozenTurns == 5) {
                    monster.Ailment = Ailment.Normal;
                    frozenTurns = 0;
                }
                else {
                    Random r = new Random();
                    if (r.Next(0, 100) < 15) {
                        monster.Ailment = Ailment.Normal;
                        frozenTurns = 0;
                    }
                    else {
                        frozenTurns++;
                    }
                }
            }
            if (monster.Ailment == Ailment.Frenzied) {
                Random r = new Random();
                if (r.Next(0, 100) < 20) {
                    monster.Ailment = Ailment.Normal;
                }
            }
        }
    }
}
