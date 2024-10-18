using HarmonyLib;
using System;
using System.Collections;
using UnityEngine;

//namespace larger_hotpot_idNS
//{
//    public class larger_hotpot_id : Mod
//    {
//        public override void Ready()
//        {
//            Logger.Log("Ready!");
//        }
//    }
//}

namespace larger_hotpot_idNS
{
    public class larger_hotpot_id : Mod
   {
        public override void Ready()
        {
            WorldManager.instance.GameDataLoader.AddCardToSetCardBag(SetCardBagType.AdvancedBuildingIdea, "blueprint_large_hotpot_id", 1);
            Logger.Log("Ready!");
        }
    }


        public class large : Hotpot
        {
            public int MaxFoodValue = 50000;

            public override bool DetermineCanHaveCardsWhenIsRoot => true;

            protected override bool CanHaveCard(CardData otherCard)
            {
                if (otherCard.MyCardType == CardType.Food)
                {
                    return true;
                }
                return false;
            }

            public override bool CanHaveCardsWhileHasStatus()
            {
                return true;
            }

            public override void UpdateCard()
            {
                MyGameCard.SpecialValue = FoodValue;
                MyGameCard.SpecialIcon.sprite = SpriteManager.instance.FoodIcon;
                if (!MyGameCard.HasParent || MyGameCard.Parent.CardData is HeavyFoundation)
                {
                    if (MyGameCard.HasChild && !MyGameCard.TimerRunning && !(MyGameCard.Child.CardData is Hotpot))
                    {
                        MyGameCard.StartTimer(10f, CookFood, SokLoc.Translate("card_hotpot_name"), GetActionId("CookFood"));
                    }
                    if (!MyGameCard.HasChild && MyGameCard.TimerRunning)
                    {
                        MyGameCard.CancelTimer(GetActionId("CookFood"));
                    }
                }
                GameCard rootCard = MyGameCard.GetRootCard();
                if (rootCard != null && rootCard.CardData is MessHall)
                {
                    MyGameCard.CancelTimer(GetActionId("CookFood"));
                }
                if (FoodValue > 0)
                {
                    descriptionOverride = "";
                }
                base.UpdateCard();
            }

            [TimedAction("cook_food")]
            public void CookFood()
            {
                foreach (GameCard childCard in MyGameCard.GetChildCards())
                {
                    if (childCard.CardData is Hotpot)
                    {
                        continue;
                    }
                    if (childCard.SpecialValue.HasValue && FoodValue + childCard.SpecialValue <= MaxFoodValue)
                    {
                        FoodValue += childCard.SpecialValue.GetValueOrDefault();
                        childCard.DestroyCard(spawnSmoke: true);
                        continue;
                    }
                    if (FoodValue != MaxFoodValue && childCard.CardData is Food food)
                    {
                        int num = MaxFoodValue - FoodValue;
                        FoodValue = MaxFoodValue;
                        food.FoodValue -= num;
                    }
                    childCard.RemoveFromParent();
                    break;
                }
            }
        }
    
}