using System;

//using Terminus.Core.Configs;
using Terminus.Core.Helper;
using UnityEngine;
//using Terminus.Game.SaveLoad;
using BrightFish;

namespace Terminus.Game.Messages
{
    public sealed class MessageBus
    {
        //public static readonly Message<string> NewPlayerCreated = new Message<string>();
        //public static readonly Message<GameLoadedMessageArgs> GameLoaded = new Message<GameLoadedMessageArgs>();
        
        //public static readonly Message<PveBattleStartRequestMessageArgs> PveBattleStartRequest = new Message<PveBattleStartRequestMessageArgs>();
        //public static readonly Message<MessageArgs> PveBattleStartResponse = new Message<MessageArgs>();
        
        //public static readonly Message<PveBattleFinishedRequestMessageArgs> PveBattleFinishedRequest = new Message<PveBattleFinishedRequestMessageArgs>();
        //public static readonly Message<PveBattleFinishedResponseMessageArgs> PveBattleFinishedResponse = new Message<PveBattleFinishedResponseMessageArgs>();
        
        //public static readonly Message<PveBattleSurrenderMessageArgs> PveBattleSurrender = new Message<PveBattleSurrenderMessageArgs>();
        
        //public static readonly Message<MissionDifficultyUnlockShownEventArgs> PveMissionDifficultyUnlockShown = new Message<MissionDifficultyUnlockShownEventArgs>();
        //public static readonly Message<MissionEventArgs> PveMissionUnlockShown = new Message<MissionEventArgs>();
        //public static readonly Message PurchaseSupplies = new Message();
        
        //public static readonly Message<PlayerResourcesChangedMessageArgs> ResourcesChanged = new Message<PlayerResourcesChangedMessageArgs>();
        
        //public static readonly Message<CardAssignedToSlotMessageArgs> UnitAssignedToSlot = new Message<CardAssignedToSlotMessageArgs>();
        //public static readonly Message<CardAssignedToSlotMessageArgs> CardAssignedToSlot = new Message<CardAssignedToSlotMessageArgs>();
        //public static readonly Message<NewCardsInfoShownMessageArgs> CardsInfoShown = new Message<NewCardsInfoShownMessageArgs>();
        //public static readonly Message<CardUpgradeMessageArgs> CardUpgrade = new Message<CardUpgradeMessageArgs>();
        
        //public static readonly Message<MessageArgs> MissionsMapShown = new Message<MessageArgs>();
        
        //public static readonly Message<MineCollectedMessageArgs> MineCollected = new Message<MineCollectedMessageArgs>();
        //public static readonly Message<MineUpgradedMessageArgs> MineUpgraded = new Message<MineUpgradedMessageArgs>();
        //public static readonly Message<MineRateUpgradedMessageArgs> MineRateUpgraded = new Message<MineRateUpgradedMessageArgs>();

        //public static readonly Message<EventRewardPurchaseEventArgs> EventCardPurchaseRequest = new Message<EventRewardPurchaseEventArgs>();
        //public static readonly Message<EventRewardPurchaseEventArgs> EventRewardPurchaseResponse = new Message<EventRewardPurchaseEventArgs>();
        
        //public static readonly Message<MessageArgs> BuildingEventsRestartRequest = new Message<MessageArgs>();
        //public static readonly Message<MessageArgs> BuildingEventsRestartResponse = new Message<MessageArgs>();
        
        //public static readonly Message<MessageArgs> MenhirCardsRequested = new Message<MessageArgs>();
        //public static readonly Message<MenhirCardsGrantedMessageArgs> MenhirCardsGranted = new Message<MenhirCardsGrantedMessageArgs>();
        //public static readonly Message MengirFlaskBuyed = new Message();

        //public static readonly Message<OpenChestRequestMessageArgs> OpenChestRequest = new Message<OpenChestRequestMessageArgs>();
        //public static readonly Message<OpenChestResponseMessageArgs> OpenChestResponse = new Message<OpenChestResponseMessageArgs>();
        
        //public static readonly Message<MessageArgs> OpenDailyChestRequest = new Message<MessageArgs>();
        //public static readonly Message<OpenChestResponseMessageArgs> OpenDailyChestResponse = new Message<OpenChestResponseMessageArgs>();
        
        //public static readonly Message<MessageArgs> OpenTokensChestRequest = new Message<MessageArgs>();
        //public static readonly Message<OpenTokensChestResponseMessageArgs> OpenTokensChestResponse = new Message<OpenTokensChestResponseMessageArgs>();
        
        //public static readonly Message<ReceiveDailyQuestRewardMessageArgs> ReceiveDailyQuestReward = new Message<ReceiveDailyQuestRewardMessageArgs>();
        
        //public static readonly Message<ComicsShownMessageArgs> ComicsShown = new Message<ComicsShownMessageArgs>();
        
        //public static readonly Message<TutorialStageFinishedMessageArgs> TutorialFinished = new Message<TutorialStageFinishedMessageArgs>();
        //public static readonly Message<TutorialStageFinishedMessageArgs> TutorialStageFinished = new Message<TutorialStageFinishedMessageArgs>();
        //public static readonly Message MandatoryTutorialsFinished = new Message();
        
        //public static readonly Message<MessageArgs> PvpTutorialStageFinished = new Message<MessageArgs>();
        
        //public static readonly Message<MessageArgs> SearchPvpOpponentsRequest = new Message<MessageArgs>();
        //public static readonly Message<SearchPvpOpponentsResponseEventArgs> SearchPvpOpponentsResponse = new Message<SearchPvpOpponentsResponseEventArgs>();
        
        //public static readonly Message<PvpBattleStartRequestMessageArgs> PvpBattleStartRequest = new Message<PvpBattleStartRequestMessageArgs>();
        //public static readonly Message<PvpBattleStartResponseMessageArgs> PvpBattleStartResponse = new Message<PvpBattleStartResponseMessageArgs>();
        
        //public static readonly Message<PvpBattleFinishedRequestMessageArgs> PvpBattleFinishedRequest = new Message<PvpBattleFinishedRequestMessageArgs>();
        //public static readonly Message<PvpBattleFinishedResponseMessageArgs> PvpBattleFinishedResponse = new Message<PvpBattleFinishedResponseMessageArgs>();
        
        //public static readonly Message<PvpBattleSurrenderMessageArgs> PvpBattleSurrender = new Message<PvpBattleSurrenderMessageArgs>();
        
        //public static readonly Message<PvpLeagueFinishedMessageArgs> PvpLeagueFinished = new Message<PvpLeagueFinishedMessageArgs>();
        //public static readonly Message<MessageArgs> PvpHubOpened = new Message<MessageArgs>();
        
        //public static readonly Message<ShopPurchaseRequestMessageArgs> ShopPurchaseRequest = new Message<ShopPurchaseRequestMessageArgs>();
        //public static readonly Message<ShopPurchaseResponseMessageArgs> ShopPurchaseResponse = new Message<ShopPurchaseResponseMessageArgs>();
        //public static readonly Message<InAppPurchaseMessageArgs> InAppPurchase = new Message<InAppPurchaseMessageArgs>();
        
        //public static readonly Message<UpdateDailyQuestRequestMessageArgs> UpdateDailyQuestRequest = new Message<UpdateDailyQuestRequestMessageArgs>();
        //public static readonly Message<UpdateDailyQuestResponseMessageArgs> UpdateDailyQuestResponse = new Message<UpdateDailyQuestResponseMessageArgs>();
        
        //public static readonly Message<QuestState, QuestState> QuestReplaced = new Message<QuestState, QuestState>();

        public static readonly Message<int> OnBubbleDestroy = new Message<int>();
        public static readonly Message<int> OnBubbleColorMatch = new Message<int>();

		public static readonly Message<Fish, ColorType, Vector3> OnFishSpawned = new Message<Fish, ColorType, Vector3>();
		public static readonly Message<Fish, ColorType, Vector3> OnFishDead = new Message<Fish, ColorType, Vector3>();
		public static readonly Message OnFishDying = new Message();

		public static readonly Message<Fish, ColorType, Vector3> OnFishRescued = new Message<Fish, ColorType, Vector3>();
		public static readonly Message<Fish, ColorType, Vector3> OnFishFinishedSmiling = new Message<Fish, ColorType, Vector3>();

        public static readonly Message<int> OnFoodDestroy = new Message<int>();

		public static readonly Message OnGameInit = new Message();
		public static readonly Message OnGameStart = new Message();
		public static readonly Message<bool> OnGamePause = new Message<bool>();
		public static readonly Message<bool> OnGameStop = new Message<bool>();

		public static readonly Message OnLevelComplete = new Message();
        public static readonly Message OnLevelFailed = new Message();

        public static readonly Message OnPlayerLivesOut = new Message();
    }

    //public sealed class PvpBattleSurrenderMessageArgs
    //    : MessageArgs
    //{
    //    public PvpBattleSurrenderMessageArgs(string opponentId)
    //    {
    //        OpponentId = opponentId;
    //    }

    //    public string OpponentId { get; }
    //}

    //public sealed class PveBattleSurrenderMessageArgs
    //    : MessageArgs
    //{
    //    public PveBattleSurrenderMessageArgs(string missionId, MissionDifficulty missionDifficulty)
    //    {
    //        MissionId = missionId;
    //        MissionDifficulty = missionDifficulty;
    //    }

    //    public string MissionId { get; }
    //    public MissionDifficulty MissionDifficulty { get; }
    //}

    public class MessageArgs
    {
        public static readonly MessageArgs None = new MessageArgs();

        public override string ToString()
        {
            return "()";
        }
    }
}