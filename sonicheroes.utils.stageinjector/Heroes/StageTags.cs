using System;
using SonicHeroes.Utils.StageInjector.Common.Shared.Enums;

namespace SonicHeroes.Utils.StageInjector.Heroes
{
    /// <summary>
    /// Tools for categorizing individual action stages based on their ID.
    /// </summary>
    public class StageTags
    {
        /// <summary>
        /// Defines individual stage properties for each stage in question.
        /// These properties are our own, custom defined and exist to simply help us classify individual levels.
        /// </summary>
        [Flags]
        public enum StageTag
        {
            /// <summary>
            /// The stage is either a team battle stage, enemy rush or a boss stage.
            /// </summary>
            Battle = 1,

            /// <summary>
            /// The stage is a tutorial stage.
            /// </summary>
            Tutorial = 2,

            /// <summary>
            /// The stage is a bobsled race stage.
            /// </summary>
            Bobsled = 4,

            /// <summary>
            /// The stage is a bonus stage.
            /// </summary>
            Bonus = 8,

            /// <summary>
            /// This stage is a special stage.
            /// </summary>
            Special = 16,

            /// <summary>
            /// This stage is exclusive to an individual team.
            /// </summary>
            Exclusive = 32,

            /// <summary>
            /// This stage is a Two Player stage.
            /// </summary>
            TwoPlayer = 64,

            /// <summary>
            /// This stage is a ring race.
            /// </summary>
            RingRace = 128
        }

        /// <summary>
        /// Converts an individual stage ID (number in RAM) into a set of tags which categorize the individual stage.
        /// </summary>
        /// <param name="stageId">The stage ID for the individual stage.</param>
        /// <returns>A list of individual tags for a stage.</returns>
        public static StageTag CategorizeStage(int stageId)
        {
            // Set default tag.
            StageTag stageTag = 0;

            // Exclusive Stages
            if (stageId == (int) StageId.TestLevel || stageId == (int) StageId.RailCanyonChaotix)
                stageTag |= StageTag.Exclusive;

            // Battle Stages
            if (IsBetween(stageId, (int)StageId.EggHawk, (int)StageId.MetalOverlord) ||
                IsBetween(stageId, (int)StageId.CityTopBattle, (int)StageId.TurtleShellBattle))
                stageTag |= StageTag.Battle;

            // Tutorial Stage
            if (stageId == (int)StageId.SeaGate)
                stageTag |= StageTag.Tutorial;

            // Bobsled Stage
            if (IsBetween(stageId, (int) StageId.SeasideCourse, (int) StageId.CasinoCourse))
                stageTag |= StageTag.Bobsled;

            // Bonus Stage
            if (IsBetween(stageId, (int) StageId.BonusStage1, (int)StageId.BonusStage7))
                stageTag |= StageTag.Bonus;

            // Two Player
            if (IsBetween(stageId, (int) StageId.SeasideHill2P, (int) StageId.EggFleetExpert) || 
                IsBetween(stageId, (int)StageId.SeasideCourse, (int)StageId.CasinoCourse) || 
                IsBetween(stageId, (int)StageId.SpecialStageMultiplayer1, (int)StageId.SpecialStageMultiplayer3))
                stageTag |= StageTag.TwoPlayer;

            // Special Stage
            if (IsBetween(stageId, (int)StageId.EmeraldChallenge1, (int)StageId.EmeraldChallenge7) ||
                IsBetween(stageId, (int)StageId.SpecialStageMultiplayer1, (int)StageId.SpecialStageMultiplayer3))
                stageTag |= StageTag.Special;

            return stageTag;
        }

        /// <summary>
        /// Checks whether a number lies between a certain range of numbers.
        /// </summary>
        /// <param name="value">The number to compare.</param>
        /// <param name="minimum">The minimum value to check.</param>
        /// <param name="maximum">The maximum value to check.</param>
        /// <returns>The number</returns>
        public static bool IsBetween(int value, int minimum, int maximum)
        {
            return value >= minimum && value <= maximum;
        }
    }
}
