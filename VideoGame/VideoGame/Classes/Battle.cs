using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.InputListeners;

namespace VideoGame.Classes
{
    public enum State
    {
        Battling,
        Won,
        Loss,
        Ran
    }

    public enum Selection
    {
        None,
        Attack,
        Item,
        Party,
        Run
    }

    class Battle
    {
        public Character User;
        public Character Opponent;
        public Monster CurrentUserMonster;
        public Monster CurrentOpponentMonster;
        private int boxSize, partySize;
        public Selection Selection = Selection.None;
        public State BattleState = State.Battling;
        public bool battleOver;
        public bool battleStart;
        private bool caught;
        private bool drawBattleButtons, drawMoves, drawInventory, drawItems, drawParty;
        public static Button AttackButton, RunButton, InventoryButton, PartyButton;
        public Move SelectedMove;
        public Monster SelectedMonster;
        public Medicine SelectedMedicine;
        public Capture SelectedCapture;
        /// <summary>
        /// Battle with a trainer
        /// </summary>
        public Battle(Character user, Character opponent)
        {
            User = user;
            Opponent = opponent;
            CurrentUserMonster = User.Monsters[0];
            User.CurrentMonster = user.Monsters[0];
            CurrentOpponentMonster = Opponent.Monsters[0];
            SetupButtons();
            battleStart = true;
        }

        /// <summary>
        /// Battle with a wild monster
        /// </summary>
        public Battle(Character user, Monster opponent)
        {
            User = user;
            CurrentUserMonster = User.Monsters[0];
            CurrentOpponentMonster = opponent;
            SetupButtons();
            battleStart = true;
        }

        public void Attack(Monster user, Monster opponent, Move chosen)
        {
            //Execute chosen move here
            chosen.Execute(user, opponent);
            //Wait for the move to complete
            //choose opponent move here with ai
        }

        public void Run(Monster user, Monster opponent)
        {
            int a = user.Stats.Speed;
            int b = opponent.Stats.Speed / 4;
            int c = 0;

            int f = ((a * 32) / b) + (30 * c);
            if (f < 255)
            {
                Random rand = new Random();
                if (rand.Next(0, 255) < f)
                {
                    BattleState = State.Ran;
                    battleOver = true;
                }
            }
            else
            {
                BattleState = State.Ran;
                battleOver = true;
            }
        }

        public void Capture(Monster opponent)
        {

        }

        public void LoopTurns(MouseState cur, MouseState prev)
        {
            UpdateButtons(cur, prev);
            switch (Selection)
            {
                case Selection.Attack:
                    if (Opponent != null)
                    {
                        int OpponentMonstersDead = 0;
                        int UserMonstersDead = 0;
                        foreach (var m in Opponent.Monsters)
                        {
                            if (m.IsDead)
                            {
                                OpponentMonstersDead++;
                            }
                        }
                        foreach (var m in User.Monsters)
                        {
                            if (m.IsDead)
                            {
                                UserMonstersDead++;
                            }
                        }
                        if (OpponentMonstersDead != Opponent.Monsters.Count && UserMonstersDead != User.Monsters.Count)
                        {
                            drawBattleButtons = true;
                            //UpdateButtons(cur, prev);
                        }
                        else
                        {
                            battleOver = true;
                        }
                    }
                    else
                    {
                        if (!CurrentUserMonster.IsDead && !CurrentOpponentMonster.IsDead)
                        {
                            drawBattleButtons = true;
                            //UpdateButtons(cur, prev);
                        }
                        else
                        {
                            battleOver = true;
                        }
                    }
                    break;
                case Selection.Item:
                    break;
                case Selection.Party:
                    break;
                case Selection.Run:
                    Run(CurrentUserMonster, CurrentOpponentMonster);
                    break;
            }

        }

        public void Update(MouseState cur, MouseState prev)
        {
            if (battleStart)
            {
                partySize = User.Monsters.Count;
                boxSize = User.Box.Count;
                //Store stats so the battle won't alter the stats permanently
                CurrentUserMonster.PreviousStats = CurrentUserMonster.Stats;
                CurrentOpponentMonster.PreviousStats = CurrentOpponentMonster.Stats;

                //Get Ability effects here
                CurrentUserMonster.Ability.GetEffects(CurrentUserMonster, CurrentOpponentMonster);
                CurrentOpponentMonster.Ability.GetEffects(CurrentOpponentMonster, CurrentUserMonster);
                battleStart = false;
                drawBattleButtons = true;
            }
            if (!battleOver)
            {
                //Choose action here, wether its an attack, using an item or switching out a monster
                LoopTurns(cur, prev);
            }
            else if (battleOver)
            {
                drawBattleButtons = false;
                //Restore the stats when the battle is over, or when the monster has been switched out
                CurrentUserMonster.Stats = CurrentUserMonster.PreviousStats;
                CurrentOpponentMonster.Stats = CurrentOpponentMonster.PreviousStats;
                //Add experience here so the stats will still be updated if the monster levels up
            }
        }


        public void Draw(SpriteBatch batch, Character player)
        {
            if (!battleStart)
            {
                Drawer.DrawBattle(batch, CurrentUserMonster, CurrentOpponentMonster);
                if (drawBattleButtons)
                {
                    DrawButtons(batch);
                    switch (Selection)
                    {
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
        public void SetupButtons()
        {
            int buttonPos = 0;

            AttackButton = new Button(new Rectangle(buttonPos, ContentLoader.GrassyBackground.Height,
                ContentLoader.Button.Width, ContentLoader.Button.Height), ContentLoader.Button, "Attack", ContentLoader.Arial);
            InventoryButton = new Button(new Rectangle((int)(buttonPos + 64), ContentLoader.GrassyBackground.Height,
                ContentLoader.Button.Width, ContentLoader.Button.Height), ContentLoader.Button, "Items", ContentLoader.Arial);
            PartyButton = new Button(new Rectangle((int)(buttonPos + 128), ContentLoader.GrassyBackground.Height,
                ContentLoader.Button.Width, ContentLoader.Button.Height), ContentLoader.Button, "Party", ContentLoader.Arial);
            RunButton = new Button(new Rectangle((int)(buttonPos + 192), ContentLoader.GrassyBackground.Height,
                ContentLoader.Button.Width, ContentLoader.Button.Height), ContentLoader.Button, "Run", ContentLoader.Arial);
        }

        public void UpdateButtons(MouseState cur, MouseState prev)
        {
            var userSpeed = CurrentUserMonster.Stats.Speed;
            var opponentSpeed = CurrentOpponentMonster.Stats.Speed;

            AttackButton.Update(cur, prev);
            InventoryButton.Update(cur, prev);
            PartyButton.Update(cur, prev);
            RunButton.Update(cur, prev);

            if (AttackButton.IsClicked(cur, prev))
            {
                Selection = Selection.Attack;
                drawMoves = true;
                drawInventory = false;
                drawParty = false;
                Drawer.DrawCapture = false;
                Drawer.DrawMedicine = false;
                //Add attack here
            }
            if (InventoryButton.IsClicked(cur, prev))
            {
                Selection = Selection.Item;
                drawMoves = false;
                drawInventory = true;
                drawParty = false;
                Drawer.DrawCapture = false;
                Drawer.DrawMedicine = false;
                //Add party here
            }
            if (PartyButton.IsClicked(cur, prev))
            {
                Selection = Selection.Party;
                drawMoves = false;
                drawInventory = false;
                drawParty = true;
                Drawer.DrawCapture = false;
                Drawer.DrawMedicine = false;
                //Add party here
            }
            if (RunButton.IsClicked(cur, prev))
            {
                Selection = Selection.Run;
            }
            GetSelected(cur, prev);
        }

        public void GetSelected(MouseState cur, MouseState prev)
        {
            var button = Drawer.GetClickedButton();
            if (button != null && button.IsHeld(cur))
            {
                if (drawMoves)
                {
                    foreach (var m in CurrentUserMonster.KnownMoves)
                    {
                        if (m.Name == button.Text)
                        {
                            if (m.Uses != 0)
                            {
                                SelectedMove = m;
                            }
                        }
                    }
                    Attack(CurrentUserMonster, CurrentOpponentMonster, SelectedMove);
                }
                if (drawParty)
                {
                    foreach (var m in User.Monsters)
                    {
                        if (m.Name == button.Text)
                        {
                            SelectedMonster = m;
                        }
                    }
                }
                if (Drawer.DrawMedicine)
                {
                    foreach (var m in User.Inventory.Medicine)
                    {
                        if (m.Value.Name == button.Text)
                        {
                            SelectedMedicine = m.Value;
                        }
                    }
                    if (SelectedMedicine != null)
                    {
                        SelectedMedicine.Use(CurrentUserMonster);
                        Drawer.DrawMedicine = false;
                        SelectedMedicine = null;
                    }
                }
                if (Drawer.DrawCapture)
                {
                    foreach (var m in User.Inventory.Captures)
                    {
                        if (m.Value.Name == button.Text)
                        {
                            SelectedCapture = m.Value;
                        }
                    }
                    if (SelectedCapture != null)
                    {
                        SelectedCapture.Use(CurrentOpponentMonster, User);
                        if (User.Monsters.Count == partySize)
                        {
                            if (User.Box.Count != boxSize)
                            {
                                BattleState = State.Won;
                                battleOver = true;
                            }
                        }
                        else
                        {
                            BattleState = State.Won;
                            battleOver = true;
                        }

                        Drawer.DrawCapture = false;
                        SelectedCapture = null;
                    }
                }
            }
        }

        public void DrawButtons(SpriteBatch batch)
        {
            AttackButton.Draw(batch);
            InventoryButton.Draw(batch);
            PartyButton.Draw(batch);
            RunButton.Draw(batch);
        }
    }
}
