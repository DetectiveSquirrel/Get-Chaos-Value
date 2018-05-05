using System;
using System.Reflection;

namespace GetValue.Enums
{
    /// <summary>
    /// This attribute is used to represent a string value
    /// for a value in an enum.
    /// </summary>
    public class StringValueAttribute : Attribute
    {

        #region Properties

        /// <summary>
        /// Holds the stringvalue for a value in an enum.
        /// </summary>
        public string StringValue { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }

        #endregion

    } 


    public static class Enchants
    {    /// <summary>
        /// Will get the string value for a given enums value, this will
        /// only work if you assign the StringValue attribute to
        /// the items in your enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                                                     typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }
        public enum Helmet
        {
            [StringValue("25% increased Abyssal Cry Damage")]
            EnchantmentAbyssalCryDamage1, //Merciless Labyrinth

            [StringValue("40% increased Abyssal Cry Damage")]
            EnchantmentAbyssalCryDamage2, //Eternal Labyrinth

            [StringValue("24% increased Abyssal Cry Duration")]
            EnchantmentAbyssalCryDuration1, //Merciless Labyrinth

            [StringValue("36% increased Abyssal Cry Duration")]
            EnchantmentAbyssalCryDuration2, //Eternal Labyrinth

            [StringValue("Ancestral Protector Totem grants 12% increased Attack Speed while Active")]
            EnchantmentAncestorTotemAttackSpeed1, //Merciless Labyrinth

            [StringValue("Ancestral Protector Totem grants 18% increased Attack Speed while Active")]
            EnchantmentAncestorTotemAttackSpeed2, //Eternal Labyrinth

            [StringValue("Ancestral Protector Totem deals 25% increased Damage")]
            EnchantmentAncestorTotemDamage1, //Merciless Labyrinth

            [StringValue("Ancestral Protector Totem deals 40% increased Damage")]
            EnchantmentAncestorTotemDamage2, //Eternal Labyrinth

            [StringValue("+24% to Ancestral Protector Totem Elemental Resistances")]
            EnchantmentAncestorTotemElementalResistances1, //Merciless Labyrinth

            [StringValue("+36% to Ancestral Protector Totem Elemental Resistances")]
            EnchantmentAncestorTotemElementalResistances2, //Eternal Labyrinth

            [StringValue("12% increased Ancestral Protector Totem Placement Speed")]
            EnchantmentAncestorTotemPlacementSpeed1, //Merciless Labyrinth

            [StringValue("18% increased Ancestral Protector Totem Placement Speed")]
            EnchantmentAncestorTotemPlacementSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Ancestral Warchief Totem Damage")]
            EnchantmentAncestorTotemSlamDamage1, //Merciless Labyrinth

            [StringValue("40% increased Ancestral Warchief Totem Damage")]
            EnchantmentAncestorTotemSlamDamage2, //Eternal Labyrinth

            [StringValue("Ancestral Warchief Totem grants 20% increased Melee Damage while Active")]
            EnchantmentAncestorTotemSlamMeleeDamage1, //Merciless Labyrinth

            [StringValue("Ancestral Warchief Totem grants 30% increased Melee Damage while Active")]
            EnchantmentAncestorTotemSlamMeleeDamage2, //Eternal Labyrinth

            [StringValue("8% increased Ancestral Warchief Totem Area of Effect")]
            EnchantmentAncestorTotemSlamRadius1, //Merciless Labyrinth

            [StringValue("12% increased Ancestral Warchief Totem Area of Effect")]
            EnchantmentAncestorTotemSlamRadius2, //Eternal Labyrinth

            [StringValue("10% reduced Anger Mana Reservation")]
            EnchantmentAngerReservationCost1, //Merciless Labyrinth

            [StringValue("15% reduced Anger Mana Reservation")]
            EnchantmentAngerReservationCost2, //Eternal Labyrinth

            [StringValue("Animated Guardians deal 25% increased Damage")]
            EnchantmentAnimateGuardianDamage1_, //Merciless Labyrinth

            [StringValue("Animated Guardians deal 40% increased Damage")]
            EnchantmentAnimateGuardianDamage2, //Eternal Labyrinth

            [StringValue("+24% to Animated Guardian Elemental Resistances")]
            EnchantmentAnimateGuardianElementalResistances1, //Merciless Labyrinth

            [StringValue("+36% to Animated Guardian Elemental Resistances")]
            EnchantmentAnimateGuardianElementalResistances2__, //Eternal Labyrinth

            [StringValue("16% chance to create an additional Animate Weapon copy")]
            EnchantmentAnimateWeaponChanceToCreateAdditionalCopy1, //Merciless Labyrinth

            [StringValue("24% chance to create an additional Animate Weapon copy")]
            EnchantmentAnimateWeaponChanceToCreateAdditionalCopy2, //Eternal Labyrinth

            [StringValue("Animated Weapons deal 25% increased Damage")]
            EnchantmentAnimateWeaponDamage1, //Merciless Labyrinth

            [StringValue("Animated Weapons deal 40% increased Damage")]
            EnchantmentAnimateWeaponDamage2, //Eternal Labyrinth

            [StringValue("20% increased Animate Weapon Duration")]
            EnchantmentAnimateWeaponDuration1_, //Merciless Labyrinth

            [StringValue("30% increased Animate Weapon Duration")]
            EnchantmentAnimateWeaponDuration2_,                         //Eternal Labyrinth

            [StringValue("25% increased Arc Damage")]
            EnchantmentArcDamage1, //Merciless Labyrinth

            [StringValue("40% increased Arc Damage")]
            EnchantmentArcDamage2, //Eternal Labyrinth

            [StringValue("Arc Chains an additional 2 times")]
            EnchantmentArcNumOfAdditionalProjectilesInChain1, //Merciless Labyrinth

            [StringValue("Arc Chains an additional 3 times")]
            EnchantmentArcNumOfAdditionalProjectilesInChain2, //Eternal Labyrinth

            [StringValue("Arc has +20% chance to Shock")]
            EnchantmentArcShockChance1, //Merciless Labyrinth

            [StringValue("Arc has +30% chance to Shock")]
            EnchantmentArcShockChance2, //Eternal Labyrinth

            [StringValue("24% increased Arctic Armour Buff Effect")]
            EnchantmentArcticArmourBuffEffect1, //Merciless Labyrinth

            [StringValue("36% increased Arctic Armour Buff Effect")]
            EnchantmentArcticArmourBuffEffect2, //Eternal Labyrinth

            [StringValue("20% reduced Arctic Armour Mana Reservation")]
            EnchantmentArcticArmourManaReservation1, //Merciless Labyrinth

            [StringValue("30% reduced Arctic Armour Mana Reservation")]
            EnchantmentArcticArmourManaReservation2, //Eternal Labyrinth

            [StringValue("25% increased Arctic Breath Damage")]
            EnchantmentArcticBreathDamage1, //Merciless Labyrinth

            [StringValue("40% increased Arctic Breath Damage")]
            EnchantmentArcticBreathDamage2, //Eternal Labyrinth

            [StringValue("24% increased Arctic Breath Duration")]
            EnchantmentArcticBreathDuration1, //Merciless Labyrinth

            [StringValue("36% increased Arctic Breath Duration")]
            EnchantmentArcticBreathDuration2, //Eternal Labyrinth

            [StringValue("8% increased Arctic Breath Area of Effect")]
            EnchantmentArcticBreathRadius1, //Merciless Labyrinth

            [StringValue("12% increased Arctic Breath Area of Effect")]
            EnchantmentArcticBreathRadius2, //Eternal Labyrinth

            [StringValue("20% increased Assassin's Mark Curse Effect")]
            EnchantmentAssassinsMarkCurseEffect1, //Merciless Labyrinth

            [StringValue("30% increased Assassin's Mark Curse Effect")]
            EnchantmentAssassinsMarkCurseEffect2, //Eternal Labyrinth

            [StringValue("30% increased Assassin's Mark Duration")]
            EnchantmentAssassinsMarkDuration1, //Merciless Labyrinth

            [StringValue("45% increased Assassin's Mark Duration")]
            EnchantmentAssassinsMarkDuration2, //Eternal Labyrinth

            [StringValue("25% increased Ball Lightning Damage")]
            EnchantmentBallLightningDamage1, //Merciless Labyrinth

            [StringValue("40% increased Ball Lightning Damage")]
            EnchantmentBallLightningDamage2, //Eternal Labyrinth

            [StringValue("30% reduced Ball Lightning Projectile Speed")]
            EnchantmentBallLightningProjectileSpeed1, //Merciless Labyrinth

            [StringValue("45% reduced Ball Lightning Projectile Speed")]
            EnchantmentBallLightningProjectileSpeed2, //Eternal Labyrinth

            [StringValue("8% increased Ball Lightning Area of Effect")]
            EnchantmentBallLightningRadius1_, //Merciless Labyrinth

            [StringValue("12% increased Ball Lightning Area of Effect")]
            EnchantmentBallLightningRadius2, //Eternal Labyrinth

            [StringValue("10% increased Barrage Attack Speed")]
            EnchantmentBarrageAttackSpeed1, //Merciless Labyrinth

            [StringValue("15% increased Barrage Attack Speed")]
            EnchantmentBarrageAttackSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Barrage Damage")]
            EnchantmentBarrageDamage1, //Merciless Labyrinth

            [StringValue("40% increased Barrage Damage")]
            EnchantmentBarrageDamage2, //Eternal Labyrinth

            [StringValue("Barrage fires an additional Projectile")]
            EnchantmentBarrageNumOfAdditionalProjectiles2_, //Eternal Labyrinth

            [StringValue("20% increased Bear Trap Cooldown Recovery Speed")]
            EnchantmentBearTrapCooldownSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Bear Trap Cooldown Recovery Speed")]
            EnchantmentBearTrapCooldownSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Bear Trap Damage")]
            EnchantmentBearTrapDamage1, //Merciless Labyrinth

            [StringValue("40% increased Bear Trap Damage")]
            EnchantmentBearTrapDamage2, //Eternal Labyrinth

            [StringValue("60% increased Bladefall Critical Strike Chance")]
            EnchantmentBladefallCriticalStrikeChance1, //Merciless Labyrinth

            [StringValue("90% increased Bladefall Critical Strike Chance")]
            EnchantmentBladefallCriticalStrikeChance2, //Eternal Labyrinth

            [StringValue("25% increased Bladefall Damage")]
            EnchantmentBladefallDamage1, //Merciless Labyrinth

            [StringValue("40% increased Bladefall Damage")]
            EnchantmentBladefallDamage2_, //Eternal Labyrinth

            [StringValue("8% increased Bladefall Area of Effect")]
            EnchantmentBladefallRadius1, //Merciless Labyrinth

            [StringValue("12% increased Bladefall Area of Effect")]
            EnchantmentBladefallRadius2, //Eternal Labyrinth

            [StringValue("25% increased Blade Vortex Spell Damage")]
            EnchantmentBladeVortexDamage1, //Merciless Labyrinth

            [StringValue("40% increased Blade Vortex Spell Damage")]
            EnchantmentBladeVortexDamage2, //Eternal Labyrinth

            [StringValue("20% increased Blade Vortex Duration")]
            EnchantmentBladeVortexDuration1, //Merciless Labyrinth

            [StringValue("30% increased Blade Vortex Duration")]
            EnchantmentBladeVortexDuration2, //Eternal Labyrinth

            [StringValue("8% increased Blade Vortex Area of Effect")]
            EnchantmentBladeVortexRadius1, //Merciless Labyrinth

            [StringValue("12% increased Blade Vortex Area of Effect")]
            EnchantmentBladeVortexRadius2, //Eternal Labyrinth

            [StringValue("Blast Rain has a 50% chance for an additional blast")]
            EnchantmentBlastRainAdditionalBlast1, //Merciless Labyrinth

            [StringValue("Blast Rain has a 75% chance for an additional blast")]
            EnchantmentBlastRainAdditionalBlast2, //Eternal Labyrinth

            [StringValue("25% increased Blast Rain Damage")]
            EnchantmentBlastRainDamage1, //Merciless Labyrinth

            [StringValue("40% increased Blast Rain Damage")]
            EnchantmentBlastRainDamage2, //Eternal Labyrinth

            [StringValue("8% increased Blast Rain Area of Effect")]
            EnchantmentBlastRainRadius1_, //Merciless Labyrinth

            [StringValue("12% increased Blast Rain Area of Effect")]
            EnchantmentBlastRainRadius2_, //Eternal Labyrinth

            [StringValue("25% increased Blight Damage")]
            EnchantmentBlightDamage1, //Merciless Labyrinth

            [StringValue("40% increased Blight Damage")]
            EnchantmentBlightDamage2, //Eternal Labyrinth

            [StringValue("8% increased Blight Area of Effect")]
            EnchantmentBlightRadius1, //Merciless Labyrinth

            [StringValue("12% increased Blight Area of Effect")]
            EnchantmentBlightRadius2, //Eternal Labyrinth

            [StringValue("Blight has 20% increased Hinder Duration")]
            EnchantmentBlightSecondarySkillDuration1, //Merciless Labyrinth

            [StringValue("Blight has 30% increased Hinder Duration")]
            EnchantmentBlightSecondarySkillDuration2, //Eternal Labyrinth

            [StringValue("Blink Arrow and Blink Arrow Clones have 10% increased Attack Speed")]
            EnchantmentBlinkArrowAttackSpeed1, //Merciless Labyrinth

            [StringValue("Blink Arrow and Blink Arrow Clones have 15% increased Attack Speed")]
            EnchantmentBlinkArrowAttackSpeed2, //Eternal Labyrinth

            [StringValue("20% increased Blink Arrow Cooldown Recovery Speed")]
            EnchantmentBlinkArrowCooldownSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Blink Arrow Cooldown Recovery Speed")]
            EnchantmentBlinkArrowCooldownSpeed2, //Eternal Labyrinth

            [StringValue("Blink Arrow and Blink Arrow Clones have 25% increased Damage")]
            EnchantmentBlinkArrowDamage1_, //Merciless Labyrinth

            [StringValue("Blink Arrow and Blink Arrow Clones have 40% increased Damage")]
            EnchantmentBlinkArrowDamage2, //Eternal Labyrinth

            [StringValue("Blood Rage grants additional 8% increased Attack Speed")]
            EnchantmentBloodRageAttackSpeed1, //Merciless Labyrinth

            [StringValue("Blood Rage grants additional 12% increased Attack Speed")]
            EnchantmentBloodRageAttackSpeed2, //Eternal Labyrinth

            [StringValue("Blood Rage grants additional 20% chance to gain a Frenzy Charge on Kill")]
            EnchantmentBloodRageFrenzyOnKill1, //Merciless Labyrinth

            [StringValue("Blood Rage grants additional 30% chance to gain a Frenzy Charge on Kill")]
            EnchantmentBloodRageFrenzyOnKill2, //Eternal Labyrinth

            [StringValue("8% increased Bodyswap Cast Speed")]
            EnchantmentBodySwapCastSpeed1__, //Merciless Labyrinth

            [StringValue("12% increased Bodyswap Cast Speed")]
            EnchantmentBodySwapCastSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Bodyswap Damage")]
            EnchantmentBodySwapDamage1_, //Merciless Labyrinth

            [StringValue("40% increased Bodyswap Damage")]
            EnchantmentBodySwapDamage2, //Eternal Labyrinth

            [StringValue("8% increased Unearth Cast Speed")]
            EnchantmentBoneLanceCastSpeed1_, //Merciless Labyrinth

            [StringValue("12% increased Unearth Cast Speed")]
            EnchantmentBoneLanceCastSpeed2_, //Eternal Labyrinth

            [StringValue("Unearth Creates Corpses with +3 Level")]
            EnchantmentBoneLanceCorpseLevel1, //Merciless Labyrinth

            [StringValue("Unearth Creates Corpses with +5 Level")]
            EnchantmentBoneLanceCorpseLevel2__, //Eternal Labyrinth

            [StringValue("25% increased Unearth Damage")]
            EnchantmentBoneLanceDamage1, //Merciless Labyrinth

            [StringValue("40% increased Unearth Damage")]
            EnchantmentBoneLanceDamage2, //Eternal Labyrinth

            [StringValue("Bone Offering grants an additional +6% Block Chance")]
            EnchantmentBoneOfferingBlockChance1, //Merciless Labyrinth

            [StringValue("Bone Offering grants an additional +9% Block Chance")]
            EnchantmentBoneOfferingBlockChance2, //Eternal Labyrinth

            [StringValue("30% increased Bone Offering Duration")]
            EnchantmentBoneOfferingDuration1, //Merciless Labyrinth

            [StringValue("45% increased Bone Offering Duration")]
            EnchantmentBoneOfferingDuration2, //Eternal Labyrinth

            [StringValue("25% increased Burning Arrow Damage")]
            EnchantmentBurningArrowDamage1, //Merciless Labyrinth

            [StringValue("40% increased Burning Arrow Damage")]
            EnchantmentBurningArrowDamage2, //Eternal Labyrinth

            [StringValue("Burning Arrow has +20% chance to Ignite")]
            EnchantmentBurningArrowIgniteChance1, //Merciless Labyrinth

            [StringValue("Burning Arrow has +30% chance to Ignite")]
            EnchantmentBurningArrowIgniteChance2, //Eternal Labyrinth

            [StringValue("10% of Burning Arrow Physical Damage gained as Extra Fire Damage")]
            EnchantmentBurningArrowPhysicalDamagePercentToAddAsFireDamage1, //Merciless Labyrinth

            [StringValue("15% of Burning Arrow Physical Damage gained as Extra Fire Damage")]
            EnchantmentBurningArrowPhysicalDamagePercentToAddAsFireDamage2, //Eternal Labyrinth

            [StringValue("25% increased Caustic Arrow Damage")]
            EnchantmentCausticArrowDamage1, //Merciless Labyrinth

            [StringValue("40% increased Caustic Arrow Damage")]
            EnchantmentCausticArrowDamage2, //Eternal Labyrinth

            [StringValue("20% increased Caustic Arrow Duration")]
            EnchantmentCausticArrowDuration1, //Merciless Labyrinth

            [StringValue("30% increased Caustic Arrow Duration")]
            EnchantmentCausticArrowDuration2, //Eternal Labyrinth

            [StringValue("8% increased Caustic Arrow Area of Effect")]
            EnchantmentCausticArrowRadius1, //Merciless Labyrinth

            [StringValue("12% increased Caustic Arrow Area of Effect")]
            EnchantmentCausticArrowRadius2, //Eternal Labyrinth

            [StringValue("+24% to Chaos Golem Elemental Resistances")]
            EnchantmentChaosGolemElementalResistances1, //Merciless Labyrinth

            [StringValue("+36% to Chaos Golem Elemental Resistances")]
            EnchantmentChaosGolemElementalResistances2, //Eternal Labyrinth

            [StringValue("75% increased Effect of the Buff granted by your Chaos Golems")]
            EnchantmentChaosGolemPercentAdditionalPhysicalDamageReduction1_, //Merciless Labyrinth

            [StringValue("100% increased Effect of the Buff granted by your Chaos Golems")]
            EnchantmentChaosGolemPercentAdditionalPhysicalDamageReduction2, //Eternal Labyrinth

            [StringValue("25% increased Blade Flurry Damage")]
            EnchantmentChargedAttackDamage1, //Merciless Labyrinth

            [StringValue("40% increased Blade Flurry Damage")]
            EnchantmentChargedAttackDamage2, //Eternal Labyrinth

            [StringValue("6% chance to Dodge Attacks at Maximum Blade Flurry stages")]
            EnchantmentChargedAttackDodgePerStack1, //Merciless Labyrinth

            [StringValue("9% chance to Dodge Attacks at Maximum Blade Flurry stages")]
            EnchantmentChargedAttackDodgePerStack2, //Eternal Labyrinth

            [StringValue("8% increased Blade Flurry Area of Effect")]
            EnchantmentChargedAttackRadius1_, //Merciless Labyrinth

            [StringValue("12% increased Blade Flurry Area of Effect")]
            EnchantmentChargedAttackRadius2, //Eternal Labyrinth

            [StringValue("25% increased Charged Dash Damage")]
            EnchantmentChargedDash1, //Merciless Labyrinth

            [StringValue("40% increased Charged Dash Damage")]
            EnchantmentChargedDash2, //Eternal Labyrinth

            [StringValue("4% chance to Dodge Attacks if you have finished Channelling Charged Dash Recently")]
            EnchantmentChargedDashDodgeWhenFinishedChannelling1_, //Merciless Labyrinth

            [StringValue("6% chance to Dodge Attacks if you have finished Channelling Charged Dash Recently")]
            EnchantmentChargedDashDodgeWhenFinishedChannelling2, //Eternal Labyrinth

            [StringValue("+6 Radius of Charged Dash's final Damage Area")]
            EnchantmentChargedDashRadiusFinalExplosion1, //Merciless Labyrinth

            [StringValue("+9 Radius of Charged Dash's final Damage Area")]
            EnchantmentChargedDashRadiusFinalExplosion2, //Eternal Labyrinth

            [StringValue("20% reduced Clarity Mana Reservation")]
            EnchantmentClarityManaReservation1, //Merciless Labyrinth

            [StringValue("30% reduced Clarity Mana Reservation")]
            EnchantmentClarityManaReservation2, //Eternal Labyrinth

            [StringValue("10% increased Cleave Attack Speed")]
            EnchantmentCleaveAttackSpeed1, //Merciless Labyrinth

            [StringValue("15% increased Cleave Attack Speed")]
            EnchantmentCleaveAttackSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Cleave Damage")]
            EnchantmentCleaveDamage1, //Merciless Labyrinth

            [StringValue("40% increased Cleave Damage")]
            EnchantmentCleaveDamage2, //Eternal Labyrinth

            [StringValue("8% increased Cleave Area of Effect")]
            EnchantmentCleaveRadius1, //Merciless Labyrinth

            [StringValue("12% increased Cleave Area of Effect")]
            EnchantmentCleaveRadius2, //Eternal Labyrinth

            [StringValue("20% increased Cold Snap Cooldown Recovery Speed")]
            EnchantmentColdSnapCooldownSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Cold Snap Cooldown Recovery Speed")]
            EnchantmentColdSnapCooldownSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Cold Snap Damage")]
            EnchantmentColdSnapDamage1, //Merciless Labyrinth

            [StringValue("40% increased Cold Snap Damage")]
            EnchantmentColdSnapDamage2, //Eternal Labyrinth

            [StringValue("8% increased Cold Snap Area of Effect")]
            EnchantmentColdSnapRadius1, //Merciless Labyrinth

            [StringValue("12% increased Cold Snap Area of Effect")]
            EnchantmentColdSnapRadius2, //Eternal Labyrinth

            [StringValue("20% increased Conductivity Curse Effect")]
            EnchantmentConductivityCurseEffect1, //Merciless Labyrinth

            [StringValue("30% increased Conductivity Curse Effect")]
            EnchantmentConductivityCurseEffect2, //Eternal Labyrinth

            [StringValue("30% increased Conductivity Duration")]
            EnchantmentConductivityDuration1, //Merciless Labyrinth

            [StringValue("45% increased Conductivity Duration")]
            EnchantmentConductivityDuration2, //Eternal Labyrinth

            [StringValue("25% increased Contagion Damage")]
            EnchantmentContagionDamage1, //Merciless Labyrinth

            [StringValue("40% increased Contagion Damage")]
            EnchantmentContagionDamage2, //Eternal Labyrinth

            [StringValue("20% increased Contagion Duration")]
            EnchantmentContagionDuration1, //Merciless Labyrinth

            [StringValue("30% increased Contagion Duration")]
            EnchantmentContagionDuration2, //Eternal Labyrinth

            [StringValue("8% increased Contagion Area of Effect")]
            EnchantmentContagionRadius1, //Merciless Labyrinth

            [StringValue("12% increased Contagion Area of Effect")]
            EnchantmentContagionRadius2, //Eternal Labyrinth

            [StringValue("20% increased Conversion Trap Cooldown Recovery Speed")]
            EnchantmentConversionTrapCooldownSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Conversion Trap Cooldown Recovery Speed")]
            EnchantmentConversionTrapCooldownSpeed2, //Eternal Labyrinth

            [StringValue("Converted Enemies have 25% increased Damage")]
            EnchantmentConversionTrapDamage1, //Merciless Labyrinth

            [StringValue("Converted Enemies have 40% increased Damage")]
            EnchantmentConversionTrapDamage2, //Eternal Labyrinth

            [StringValue("20% increased Convocation Cooldown Recovery Speed")]
            EnchantmentConvocationCooldownSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Convocation Cooldown Recovery Speed")]
            EnchantmentConvocationCooldownSpeed2, //Eternal Labyrinth

            [StringValue("24% increased Convocation Buff Effect")]
            EnchantmentConvocationLifeRegeneration1, //Merciless Labyrinth

            [StringValue("36% increased Convocation Buff Effect")]
            EnchantmentConvocationLifeRegeneration2, //Eternal Labyrinth

            [StringValue("8% increased Cremation Cast Speed")]
            EnchantmentCorpseEruptionCastSpeed1, //Merciless Labyrinth

            [StringValue("12% increased Cremation Cast Speed")]
            EnchantmentCorpseEruptionCastSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Cremation Damage")]
            EnchantmentCorpseEruptionDamage1, //Merciless Labyrinth

            [StringValue("40% increased Cremation Damage")]
            EnchantmentCorpseEruptionDamage2, //Eternal Labyrinth

            [StringValue("Cremation can have up to 1 additional Geyser at a time")]
            EnchantmentCorpseEruptionMaximumGeysers1, //Eternal Labyrinth

            [StringValue("10% increased Cyclone Attack Speed")]
            EnchantmentCycloneAttackSpeed1, //Merciless Labyrinth

            [StringValue("15% increased Cyclone Attack Speed")]
            EnchantmentCycloneAttackSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Cyclone Damage")]
            EnchantmentCycloneDamage1_, //Merciless Labyrinth

            [StringValue("40% increased Cyclone Damage")]
            EnchantmentCycloneDamage2, //Eternal Labyrinth

            [StringValue("8% increased Dark Pact Cast Speed")]
            EnchantmentDarkPactCastSpeed1, //Merciless Labyrinth

            [StringValue("12% increased Dark Pact Cast Speed")]
            EnchantmentDarkPactCastSpeed2, //Eternal Labyrinth

            [StringValue("8% increased Dark Pact Area of Effect")]
            EnchantmentDarkPactRadius1__, //Merciless Labyrinth

            [StringValue("12% increased Dark Pact Area of Effect")]
            EnchantmentDarkPactRadius2, //Eternal Labyrinth

            [StringValue("40% increased Decoy Totem Life")]
            EnchantmentDecoyTotemLife1, //Merciless Labyrinth

            [StringValue("60% increased Decoy Totem Life")]
            EnchantmentDecoyTotemLife2, //Eternal Labyrinth

            [StringValue("16% increased Decoy Totem Area of Effect")]
            EnchantmentDecoyTotemRadius1, //Merciless Labyrinth

            [StringValue("24% increased Decoy Totem Area of Effect")]
            EnchantmentDecoyTotemRadius2, //Eternal Labyrinth

            [StringValue("Desecrate summons 2 additional corpses")]
            EnchantmentDesecrateAdditionalCorpse1, //Merciless Labyrinth

            [StringValue("Desecrate summons 3 additional corpses")]
            EnchantmentDesecrateAdditionalCorpse2, //Eternal Labyrinth

            [StringValue("20% increased Desecrate Cooldown Recovery Speed")]
            EnchantmentDesecrateCooldownSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Desecrate Cooldown Recovery Speed")]
            EnchantmentDesecrateCooldownSpeed2, //Eternal Labyrinth

            [StringValue("30% increased Despair Duration")]
            EnchantmentDespairDuration1, //Merciless Labyrinth

            [StringValue("45% increased Despair Duration")]
            EnchantmentDespairDuration2, //Eternal Labyrinth

            [StringValue("20% increased Despair Curse Effect")]
            EnchantmentDespairEffect1_, //Merciless Labyrinth

            [StringValue("30% increased Despair Curse Effect")]
            EnchantmentDespairEffect2, //Eternal Labyrinth

            [StringValue("10% reduced Determination Mana Reservation")]
            EnchantmentDeterminationManaReservation1, //Merciless Labyrinth

            [StringValue("15% reduced Determination Mana Reservation")]
            EnchantmentDeterminationManaReservation2, //Eternal Labyrinth

            [StringValue("25% increased Detonate Dead Damage")]
            EnchantmentDetonateDeadDamage1, //Merciless Labyrinth

            [StringValue("40% increased Detonate Dead Damage")]
            EnchantmentDetonateDeadDamage2, //Eternal Labyrinth

            [StringValue("Detonate Dead has a 30% chance to detonate an additional Corpse")]
            EnchantmentDetonateDeadPercentChanceToDetonateAdditionalCorpse1, //Merciless Labyrinth

            [StringValue("Detonate Dead has a 45% chance to detonate an additional Corpse")]
            EnchantmentDetonateDeadPercentChanceToDetonateAdditionalCorpse2, //Eternal Labyrinth

            [StringValue("8% increased Detonate Dead Area of Effect")]
            EnchantmentDetonateDeadRadius1, //Merciless Labyrinth

            [StringValue("12% increased Detonate Dead Area of Effect")]
            EnchantmentDetonateDeadRadius2, //Eternal Labyrinth

            [StringValue("24% increased Devouring Totem Leech per second")]
            EnchantmentDevouringTotemLeechPerSecond1, //Merciless Labyrinth

            [StringValue("36% increased Devouring Totem Leech per second")]
            EnchantmentDevouringTotemLeechPerSecond2, //Eternal Labyrinth

            [StringValue("40% increased Chance to consume an additional Corpse with Devouring Totem")]
            EnchantmentDevouringTotemPercentChanceToConsumeAdditionalCorpse1, //Merciless Labyrinth

            [StringValue("60% increased Chance to consume an additional Corpse with Devouring Totem")]
            EnchantmentDevouringTotemPercentChanceToConsumeAdditionalCorpse2, //Eternal Labyrinth

            [StringValue("20% chance for Discharge not to consume Charges")]
            EnchantmentDischargeChanceToNotConsumeCharges1, //Merciless Labyrinth

            [StringValue("30% chance for Discharge not to consume Charges")]
            EnchantmentDischargeChanceToNotConsumeCharges2, //Eternal Labyrinth

            [StringValue("25% increased Discharge Damage")]
            EnchantmentDischargeDamage1, //Merciless Labyrinth

            [StringValue("40% increased Discharge Damage")]
            EnchantmentDischargeDamage2, //Eternal Labyrinth

            [StringValue("5% increased Discharge Radius")]
            EnchantmentDischargeRadius1, //Merciless Labyrinth

            [StringValue("8% increased Discharge Radius")]
            EnchantmentDischargeRadius2, //Eternal Labyrinth

            [StringValue("14% reduced Discipline Mana Reservation")]
            EnchantmentDisciplineManaReservation1, //Merciless Labyrinth

            [StringValue("20% reduced Discipline Mana Reservation")]
            EnchantmentDisciplineManaReservation2, //Eternal Labyrinth

            [StringValue("24% increased Dominating Blow Damage")]
            EnchantmentDominatingBlowAttackDamage1, //Merciless Labyrinth

            [StringValue("36% increased Dominating Blow Damage")]
            EnchantmentDominatingBlowAttackDamage2, //Eternal Labyrinth

            [StringValue("20% increased Dominating Blow Duration")]
            EnchantmentDominatingBlowDuration1, //Merciless Labyrinth

            [StringValue("30% increased Dominating Blow Duration")]
            EnchantmentDominatingBlowDuration2, //Eternal Labyrinth

            [StringValue("Dominated Minions deal 20% increased Damage")]
            EnchantmentDominatingBlowMinionDamage1, //Merciless Labyrinth

            [StringValue("Dominated Minions deal 30% increased Damage")]
            EnchantmentDominatingBlowMinionDamage2_, //Eternal Labyrinth

            [StringValue("40% increased Lacerate Critical Strike Chance")]
            EnchantmentDoubleSlashCriticalStrikes1, //Merciless Labyrinth

            [StringValue("60% increased Lacerate Critical Strike Chance")]
            EnchantmentDoubleSlashCriticalStrikes2, //Eternal Labyrinth

            [StringValue("25% increased Lacerate Damage")]
            EnchantmentDoubleSlashDamage1, //Merciless Labyrinth

            [StringValue("40% increased Lacerate Damage")]
            EnchantmentDoubleSlashDamage2, //Eternal Labyrinth

            [StringValue("8% increased Lacerate Area of Effect")]
            EnchantmentDoubleSlashRadius1, //Merciless Labyrinth

            [StringValue("12% increased Lacerate Area of Effect")]
            EnchantmentDoubleSlashRadius2, //Eternal Labyrinth

            [StringValue("10% increased Double Strike Attack Speed")]
            EnchantmentDoubleStrikeAttackSpeed1, //Merciless Labyrinth

            [StringValue("15% increased Double Strike Attack Speed")]
            EnchantmentDoubleStrikeAttackSpeed2, //Eternal Labyrinth

            [StringValue("60% increased Double Strike Critical Strike Chance")]
            EnchantmentDoubleStrikeCriticalStrikeChance1, //Merciless Labyrinth

            [StringValue("90% increased Double Strike Critical Strike Chance")]
            EnchantmentDoubleStrikeCriticalStrikeChance2, //Eternal Labyrinth

            [StringValue("25% increased Double Strike Damage")]
            EnchantmentDoubleStrikeDamage1, //Merciless Labyrinth

            [StringValue("40% increased Double Strike Damage")]
            EnchantmentDoubleStrikeDamage2, //Eternal Labyrinth

            [StringValue("10% increased Dual Strike Attack Speed")]
            EnchantmentDualStrikeAttackSpeed1, //Merciless Labyrinth

            [StringValue("15% increased Dual Strike Attack Speed")]
            EnchantmentDualStrikeAttackSpeed2, //Eternal Labyrinth

            [StringValue("60% increased Dual Strike Critical Strike Chance")]
            EnchantmentDualStrikeCriticalStrikeChance1, //Merciless Labyrinth

            [StringValue("90% increased Dual Strike Critical Strike Chance")]
            EnchantmentDualStrikeCriticalStrikeChance2, //Eternal Labyrinth

            [StringValue("25% increased Dual Strike Damage")]
            EnchantmentDualStrikeDamage1, //Merciless Labyrinth

            [StringValue("40% increased Dual Strike Damage")]
            EnchantmentDualStrikeDamage2, //Eternal Labyrinth

            [StringValue("25% increased Earthquake Damage")]
            EnchantmentEarthquakeDamage1, //Merciless Labyrinth

            [StringValue("40% increased Earthquake Damage")]
            EnchantmentEarthquakeDamage2_, //Eternal Labyrinth

            [StringValue("20% reduced Earthquake Duration")]
            EnchantmentEarthquakeDuration1, //Merciless Labyrinth

            [StringValue("30% reduced Earthquake Duration")]
            EnchantmentEarthquakeDuration2, //Eternal Labyrinth

            [StringValue("8% increased Earthquake Area of Effect")]
            EnchantmentEarthquakeRadius1, //Merciless Labyrinth

            [StringValue("12% increased Earthquake Area of Effect")]
            EnchantmentEarthquakeRadius2, //Eternal Labyrinth

            [StringValue("10% increased Elemental Hit Attack Speed")]
            EnchantmentElementalHitAttackSpeed1, //Merciless Labyrinth

            [StringValue("15% increased Elemental Hit Attack Speed")]
            EnchantmentElementalHitAttackSpeed2, //Eternal Labyrinth

            [StringValue("Elemental Hit has +20% chance to Freeze, Shock and Ignite")]
            EnchantmentElementalHitChanceToFreezeShockIgnite1, //Merciless Labyrinth

            [StringValue("Elemental Hit has +30% chance to Freeze, Shock and Ignite")]
            EnchantmentElementalHitChanceToFreezeShockIgnite2, //Eternal Labyrinth

            [StringValue("25% increased Elemental Hit Damage")]
            EnchantmentElementalHitDamage1, //Merciless Labyrinth

            [StringValue("40% increased Elemental Hit Damage")]
            EnchantmentElementalHitDamage2, //Eternal Labyrinth

            [StringValue("20% increased Elemental Weakness Curse Effect")]
            EnchantmentElementalWeaknessCurseEffect1, //Merciless Labyrinth

            [StringValue("30% increased Elemental Weakness Curse Effect")]
            EnchantmentElementalWeaknessCurseEffect2, //Eternal Labyrinth

            [StringValue("30% increased Elemental Weakness Duration")]
            EnchantmentElementalWeaknessDuration1, //Merciless Labyrinth

            [StringValue("45% increased Elemental Weakness Duration")]
            EnchantmentElementalWeaknessDuration2, //Eternal Labyrinth

            [StringValue("20% increased Enduring Cry Cooldown Recovery Speed")]
            EnchantmentEnduringCryCooldownSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Enduring Cry Cooldown Recovery Speed")]
            EnchantmentEnduringCryCooldownSpeed2, //Eternal Labyrinth

            [StringValue("24% increased Enduring Cry Buff Effect")]
            EnchantmentEnduringCryLifeRegeneration1, //Merciless Labyrinth

            [StringValue("36% increased Enduring Cry Buff Effect")]
            EnchantmentEnduringCryLifeRegeneration2, //Eternal Labyrinth

            [StringValue("20% increased Enfeeble Curse Effect")]
            EnchantmentEnfeebleCurseEffect1, //Merciless Labyrinth

            [StringValue("30% increased Enfeeble Curse Effect")]
            EnchantmentEnfeebleCurseEffect2, //Eternal Labyrinth

            [StringValue("30% increased Enfeeble Duration")]
            EnchantmentEnfeebleDuration1, //Merciless Labyrinth

            [StringValue("45% increased Enfeeble Duration")]
            EnchantmentEnfeebleDuration2, //Eternal Labyrinth

            [StringValue("25% increased Essence Drain Damage")]
            EnchantmentEssenceDrainDamage1, //Merciless Labyrinth

            [StringValue("40% increased Essence Drain Damage")]
            EnchantmentEssenceDrainDamage2, //Eternal Labyrinth

            [StringValue("20% increased Essence Drain Duration")]
            EnchantmentEssenceDrainDuration1, //Merciless Labyrinth

            [StringValue("30% increased Essence Drain Duration")]
            EnchantmentEssenceDrainDuration2, //Eternal Labyrinth

            [StringValue("25% increased Ethereal Knives Damage")]
            EnchantmentEtherealKnivesDamage1, //Merciless Labyrinth

            [StringValue("40% increased Ethereal Knives Damage")]
            EnchantmentEtherealKnivesDamage2, //Eternal Labyrinth

            [StringValue("20% increased Ethereal Knives Projectile Speed")]
            EnchantmentEtherealKnivesProjectileSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Ethereal Knives Projectile Speed")]
            EnchantmentEtherealKnivesProjectileSpeed2_, //Eternal Labyrinth

            [StringValue("10% increased Explosive Arrow Attack Speed")]
            EnchantmentExplosiveArrowAttackSpeed1, //Merciless Labyrinth

            [StringValue("15% increased Explosive Arrow Attack Speed")]
            EnchantmentExplosiveArrowAttackSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Explosive Arrow Damage")]
            EnchantmentExplosiveArrowDamage1, //Merciless Labyrinth

            [StringValue("40% increased Explosive Arrow Damage")]
            EnchantmentExplosiveArrowDamage2, //Eternal Labyrinth

            [StringValue("8% increased Explosive Arrow Area of Effect")]
            EnchantmentExplosiveArrowRadius1, //Merciless Labyrinth

            [StringValue("12% increased Explosive Arrow Area of Effect")]
            EnchantmentExplosiveArrowRadius2, //Eternal Labyrinth

            [StringValue("8% increased Fireball Cast Speed")]
            EnchantmentFireballCastSpeed1, //Merciless Labyrinth

            [StringValue("12% increased Fireball Cast Speed")]
            EnchantmentFireballCastSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Fireball Damage")]
            EnchantmentFireballDamage1, //Merciless Labyrinth

            [StringValue("40% increased Fireball Damage")]
            EnchantmentFireballDamage2_, //Eternal Labyrinth

            [StringValue("Fireball has +20% chance to Ignite")]
            EnchantmentFireballIgniteChance1, //Merciless Labyrinth

            [StringValue("Fireball has +30% chance to Ignite")]
            EnchantmentFireballIgniteChance2, //Eternal Labyrinth

            [StringValue("8% increased Scorching Ray Cast Speed")]
            EnchantmentFireBeamCastSpeed1, //Merciless Labyrinth

            [StringValue("12% increased Scorching Ray Cast Speed")]
            EnchantmentFireBeamCastSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Scorching Ray Damage")]
            EnchantmentFireBeamDamage1, //Merciless Labyrinth

            [StringValue("40% increased Scorching Ray Damage")]
            EnchantmentFireBeamDamage2, //Eternal Labyrinth

            [StringValue("10% increased Scorching Ray beam length")]
            EnchantmentFireBeamLength1, //Merciless Labyrinth

            [StringValue("15% increased Scorching Ray beam length")]
            EnchantmentFireBeamLength2, //Eternal Labyrinth

            [StringValue("20% increased Fire Nova Cast Speed")]
            EnchantmentFireNovaMineCastSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Fire Nova Cast Speed")]
            EnchantmentFireNovaMineCastSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Fire Nova Mine Damage")]
            EnchantmentFireNovaMineDamage1, //Merciless Labyrinth

            [StringValue("40% increased Fire Nova Mine Damage")]
            EnchantmentFireNovaMineDamage2, //Eternal Labyrinth

            [StringValue("Fire Nova Mine repeats an additional 1 times")]
            EnchantmentFireNovaMineNumOfAdditionalRepeats1, //Merciless Labyrinth

            [StringValue("Fire Nova Mine repeats an additional 2 times")]
            EnchantmentFireNovaMineNumOfAdditionalRepeats2, //Eternal Labyrinth

            [StringValue("25% increased Firestorm Damage")]
            EnchantmentFireStormDamage1, //Merciless Labyrinth

            [StringValue("40% increased Firestorm Damage")]
            EnchantmentFireStormDamage2, //Eternal Labyrinth

            [StringValue("20% increased Firestorm Duration")]
            EnchantmentFirestormDuration1, //Merciless Labyrinth

            [StringValue("30% increased Firestorm Duration")]
            EnchantmentFirestormDuration2, //Eternal Labyrinth

            [StringValue("8% increased Firestorm explosion Area of Effect")]
            EnchantmentFirestormExplosionAreaOfEffect1, //Merciless Labyrinth

            [StringValue("12% increased Firestorm explosion Area of Effect")]
            EnchantmentFirestormExplosionAreaOfEffect2, //Eternal Labyrinth

            [StringValue("40% increased Fire Trap Burning Damage")]
            EnchantmentFireTrapBurningDamage1, //Merciless Labyrinth

            [StringValue("60% increased Fire Trap Burning Damage")]
            EnchantmentFireTrapBurningDamage2, //Eternal Labyrinth

            [StringValue("20% increased Fire Trap Cooldown Recovery Speed")]
            EnchantmentFireTrapCooldownSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Fire Trap Cooldown Recovery Speed")]
            EnchantmentFireTrapCooldownSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Fire Trap Damage")]
            EnchantmentFireTrapDamage1, //Merciless Labyrinth

            [StringValue("40% increased Fire Trap Damage")]
            EnchantmentFireTrapDamage2, //Eternal Labyrinth

            [StringValue("60% increased Flameblast Critical Strike Chance")]
            EnchantmentFlameblastCriticalStrikeChance1, //Merciless Labyrinth

            [StringValue("90% increased Flameblast Critical Strike Chance")]
            EnchantmentFlameblastCriticalStrikeChance2, //Eternal Labyrinth

            [StringValue("25% increased Flameblast Damage")]
            EnchantmentFlameblastDamage1, //Merciless Labyrinth

            [StringValue("40% increased Flameblast Damage")]
            EnchantmentFlameblastDamage2, //Eternal Labyrinth

            [StringValue("8% increased Flameblast Area of Effect")]
            EnchantmentFlameblastRadius1, //Merciless Labyrinth

            [StringValue("12% increased Flameblast Area of Effect")]
            EnchantmentFlameblastRadius2_, //Eternal Labyrinth

            [StringValue("20% increased Flame Dash Cooldown Recovery Speed")]
            EnchantmentFlameDashCooldownSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Flame Dash Cooldown Recovery Speed")]
            EnchantmentFlameDashCooldownSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Flame Dash Damage")]
            EnchantmentFlameDashDamage1, //Merciless Labyrinth

            [StringValue("40% increased Flame Dash Damage")]
            EnchantmentFlameDashDamage2, //Eternal Labyrinth

            [StringValue("+24% to increased Flame Golem Elemental Resistances")]
            EnchantmentFlameGolemElementalResistances1, //Merciless Labyrinth

            [StringValue("+36% to increased Flame Golem Elemental Resistances")]
            EnchantmentFlameGolemElementalResistances2, //Eternal Labyrinth

            [StringValue("100% increased Effect of the Buff granted by your Flame Golems")]
            EnchantmentFlameGolemGrantedBuffEffect1_, //Merciless Labyrinth

            [StringValue("150% increased Effect of the Buff granted by your Flame Golems")]
            EnchantmentFlameGolemGrantedBuffEffect2, //Eternal Labyrinth

            [StringValue("60% increased Flame Surge Critical Strike Chance")]
            EnchantmentFlameSurgeCriticalStrikeChance1, //Merciless Labyrinth

            [StringValue("90% increased Flame Surge Critical Strike Chance")]
            EnchantmentFlameSurgeCriticalStrikeChance2_, //Eternal Labyrinth

            [StringValue("25% increased Flame Surge Damage")]
            EnchantmentFlameSurgeDamage1, //Merciless Labyrinth

            [StringValue("40% increased Flame Surge Damage")]
            EnchantmentFlameSurgeDamage2, //Eternal Labyrinth

            [StringValue("40% increased Flame Surge Damage against Burning Enemies")]
            EnchantmentFlameSurgeVsBurningEnemies1, //Merciless Labyrinth

            [StringValue("60% increased Flame Surge Damage against Burning Enemies")]
            EnchantmentFlameSurgeVsBurningEnemies2_, //Eternal Labyrinth

            [StringValue("25% increased Flame Totem Damage")]
            EnchantmentFlameTotemDamage1, //Merciless Labyrinth

            [StringValue("40% increased Flame Totem Damage")]
            EnchantmentFlameTotemDamage2, //Eternal Labyrinth

            [StringValue("Flame Totem fires an additional Projectile")]
            EnchantmentFlameTotemNumOfAdditionalProjectiles1, //Merciless Labyrinth

            [StringValue("Flame Totem fires 2 additional Projectiles")]
            EnchantmentFlameTotemNumOfAdditionalProjectiles2, //Eternal Labyrinth

            [StringValue("20% increased Flame Totem Projectile Speed")]
            EnchantmentFlameTotemProjectileSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Flame Totem Projectile Speed")]
            EnchantmentFlameTotemProjectileSpeed2, //Eternal Labyrinth

            [StringValue("20% increased Flammability Curse Effect")]
            EnchantmentFlammabilityCurseEffect1, //Merciless Labyrinth

            [StringValue("30% increased Flammability Curse Effect")]
            EnchantmentFlammabilityCurseEffect2, //Eternal Labyrinth

            [StringValue("30% increased Flammability Duration")]
            EnchantmentFlammabilityDuration1, //Merciless Labyrinth

            [StringValue("45% increased Flammability Duration")]
            EnchantmentFlammabilityDuration2, //Eternal Labyrinth

            [StringValue("Flesh Offering grants an additional 14% increased Attack Speed")]
            EnchantmentFleshOfferingAttackSpeed1__, //Merciless Labyrinth

            [StringValue("Flesh Offering grants an additional 21% increased Attack Speed")]
            EnchantmentFleshOfferingAttackSpeed2_, //Eternal Labyrinth

            [StringValue("30% increased Flesh Offering Duration")]
            EnchantmentFleshOfferingDuration1, //Merciless Labyrinth

            [StringValue("45% increased Flesh Offering Duration")]
            EnchantmentFleshOfferingDuration2, //Eternal Labyrinth

            [StringValue("20% increased Flicker Strike Cooldown Recovery Speed")]
            EnchantmentFlickerStrikeCooldownSpeed1_, //Merciless Labyrinth

            [StringValue("30% increased Flicker Strike Cooldown Recovery Speed")]
            EnchantmentFlickerStrikeCooldownSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Flicker Strike Damage")]
            EnchantmentFlickerStrikeDamage1, //Merciless Labyrinth

            [StringValue("40% increased Flicker Strike Damage")]
            EnchantmentFlickerStrikeDamage2, //Eternal Labyrinth

            [StringValue("6% increased Flicker Strike Damage per Frenzy Charge")]
            EnchantmentFlickerStrikeDamagePerFrenzyCharge1, //Merciless Labyrinth

            [StringValue("9% increased Flicker Strike Damage per Frenzy Charge")]
            EnchantmentFlickerStrikeDamagePerFrenzyCharge2, //Eternal Labyrinth

            [StringValue("Freeze Mine causes Enemies to lose an additional 8% Cold Resistance while Frozen")]
            EnchantmentFreezeMineColdPenetration1, //Merciless Labyrinth

            [StringValue("Freeze Mine causes Enemies to lose an additional 12% Cold Resistance while Frozen")]
            EnchantmentFreezeMineColdPenetration2, //Eternal Labyrinth

            [StringValue("8% increased Freeze Mine Area of Effect")]
            EnchantmentFreezeMineRadius1, //Merciless Labyrinth

            [StringValue("12% increased Freeze Mine Area of Effect")]
            EnchantmentFreezeMineRadius2_, //Eternal Labyrinth

            [StringValue("8% increased Freezing Pulse Cast Speed")]
            EnchantmentFreezingPulseCastSpeed1, //Merciless Labyrinth

            [StringValue("12% increased Freezing Pulse Cast Speed")]
            EnchantmentFreezingPulseCastSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Freezing Pulse Damage")]
            EnchantmentFreezingPulseDamage1, //Merciless Labyrinth

            [StringValue("40% increased Freezing Pulse Damage")]
            EnchantmentFreezingPulseDamage2, //Eternal Labyrinth

            [StringValue("20% increased Freezing Pulse Projectile Speed")]
            EnchantmentFreezingPulseProjectileSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Freezing Pulse Projectile Speed")]
            EnchantmentFreezingPulseProjectileSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Frenzy Damage")]
            EnchantmentFrenzyDamage1, //Merciless Labyrinth

            [StringValue("40% increased Frenzy Damage")]
            EnchantmentFrenzyDamage2, //Eternal Labyrinth

            [StringValue("6% increased Frenzy Damage per Frenzy Charge")]
            EnchantmentFrenzyDamagePerFrenzyCharge1, //Merciless Labyrinth

            [StringValue("9% increased Frenzy Damage per Frenzy Charge")]
            EnchantmentFrenzyDamagePerFrenzyCharge2, //Eternal Labyrinth

            [StringValue("20% Chance on Frenzy to gain an additional Frenzy Charge")]
            EnchantmentFrenzyPercentChanceToGainAdditionalFrenzyCharge1, //Merciless Labyrinth

            [StringValue("30% Chance on Frenzy to gain an additional Frenzy Charge")]
            EnchantmentFrenzyPercentChanceToGainAdditionalFrenzyCharge2, //Eternal Labyrinth

            [StringValue("20% increased Frostbite Curse Effect")]
            EnchantmentFrostbiteCurseEffect1, //Merciless Labyrinth

            [StringValue("30% increased Frostbite Curse Effect")]
            EnchantmentFrostbiteCurseEffect2_, //Eternal Labyrinth

            [StringValue("30% increased Frostbite Duration")]
            EnchantmentFrostbiteDuration1, //Merciless Labyrinth

            [StringValue("45% increased Frostbite Duration")]
            EnchantmentFrostbiteDuration2, //Eternal Labyrinth

            [StringValue("25% increased Frost Blades Damage")]
            EnchantmentFrostBladesDamage1, //Merciless Labyrinth

            [StringValue("40% increased Frost Blades Damage")]
            EnchantmentFrostBladesDamage2, //Eternal Labyrinth

            [StringValue("20% increased Frost Blades Projectile Speed")]
            EnchantmentFrostBladesProjectileSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Frost Blades Projectile Speed")]
            EnchantmentFrostBladesProjectileSpeed2, //Eternal Labyrinth

            [StringValue("10% increased Frostbolt Cast Speed")]
            EnchantmentFrostBoltCastSpeed1_, //Merciless Labyrinth

            [StringValue("15% increased Frostbolt Cast Speed")]
            EnchantmentFrostBoltCastSpeed2_, //Eternal Labyrinth

            [StringValue("25% increased Frostbolt Damage")]
            EnchantmentFrostBoltDamage1, //Merciless Labyrinth

            [StringValue("40% increased Frostbolt Damage")]
            EnchantmentFrostBoltDamage2, //Eternal Labyrinth

            [StringValue("Frostbolt has +10% chance to Freeze")]
            EnchantmentFrostBoltFreezeChance1, //Merciless Labyrinth

            [StringValue("Frostbolt has +15% chance to Freeze")]
            EnchantmentFrostBoltFreezeChance2, //Eternal Labyrinth

            [StringValue("20% increased Frost Bomb Cooldown Recovery Speed")]
            EnchantmentFrostBombCooldownSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Frost Bomb Cooldown Recovery Speed")]
            EnchantmentFrostBombCooldownSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Frost Bomb Damage")]
            EnchantmentFrostBombDamage1, //Merciless Labyrinth

            [StringValue("40% increased Frost Bomb Damage")]
            EnchantmentFrostBombDamage2, //Eternal Labyrinth

            [StringValue("8% increased Frost Bomb Area of Effect")]
            EnchantmentFrostBombRadius1, //Merciless Labyrinth

            [StringValue("12% increased Frost Bomb Area of Effect")]
            EnchantmentFrostBombRadius2, //Eternal Labyrinth

            [StringValue("20% increased Frost Wall Cooldown Recovery Speed")]
            EnchantmentFrostWallCooldownSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Frost Wall Cooldown Recovery Speed")]
            EnchantmentFrostWallCooldownSpeed2, //Eternal Labyrinth

            [StringValue("24% increased Frost Wall Duration")]
            EnchantmentFrostWallDuration1, //Merciless Labyrinth

            [StringValue("36% increased Frost Wall Duration")]
            EnchantmentFrostWallDuration2, //Eternal Labyrinth

            [StringValue("25% increased Glacial Cascade Damage")]
            EnchantmentGlacialCascadeDamage1, //Merciless Labyrinth

            [StringValue("40% increased Glacial Cascade Damage")]
            EnchantmentGlacialCascadeDamage2, //Eternal Labyrinth

            [StringValue("30% of Glacial Cascade Physical Damage Converted to Cold Damage")]
            EnchantmentGlacialCascadePhysicalDamagePercentToConvertToCold1, //Merciless Labyrinth

            [StringValue("40% of Glacial Cascade Physical Damage Converted to Cold Damage")]
            EnchantmentGlacialCascadePhysicalDamagePercentToConvertToCold2, //Eternal Labyrinth

            [StringValue("8% increased Glacial Cascade Area of Effect")]
            EnchantmentGlacialCascadeRadius1, //Merciless Labyrinth

            [StringValue("12% increased Glacial Cascade Area of Effect")]
            EnchantmentGlacialCascadeRadius2_, //Eternal Labyrinth

            [StringValue("25% increased Glacial Hammer Damage")]
            EnchantmentGlacialHammerDamage1, //Merciless Labyrinth

            [StringValue("40% increased Glacial Hammer Damage")]
            EnchantmentGlacialHammerDamage2, //Eternal Labyrinth

            [StringValue("Glacial Hammer has +20% chance to Freeze")]
            EnchantmentGlacialHammerFreezeChance1, //Merciless Labyrinth

            [StringValue("Glacial Hammer has +30% chance to Freeze")]
            EnchantmentGlacialHammerFreezeChance2, //Eternal Labyrinth

            [StringValue("10% of Glacial Hammer Physical Damage gained as Extra Cold Damage")]
            EnchantmentGlacialHammerPhysicalDamagePercentToAddAsColdDamage1, //Merciless Labyrinth

            [StringValue("15% of Glacial Hammer Physical Damage gained as Extra Cold Damage")]
            EnchantmentGlacialHammerPhysicalDamagePercentToAddAsColdDamage2, //Eternal Labyrinth

            [StringValue("10% reduced Grace Mana Reservation")]
            EnchantmentGraceManaReservation1, //Merciless Labyrinth

            [StringValue("15% reduced Grace Mana Reservation")]
            EnchantmentGraceManaReservation2, //Eternal Labyrinth

            [StringValue("Ground Slam has a 16% increased angle")]
            EnchantmentGroundSlamAngle1, //Merciless Labyrinth

            [StringValue("Ground Slam has a 24% increased angle")]
            EnchantmentGroundSlamAngle2, //Eternal Labyrinth

            [StringValue("25% increased Ground Slam Damage")]
            EnchantmentGroundSlamDamage1, //Merciless Labyrinth

            [StringValue("40% increased Ground Slam Damage")]
            EnchantmentGroundSlamDamage2, //Eternal Labyrinth

            [StringValue("8% increased Ground Slam Area of Effect")]
            EnchantmentGroundSlamRadius1, //Merciless Labyrinth

            [StringValue("12% increased Ground Slam Area of Effect")]
            EnchantmentGroundSlamRadius2, //Eternal Labyrinth

            [StringValue("10% reduced Haste Mana Reservation")]
            EnchantmentHasteManaReservation1, //Merciless Labyrinth

            [StringValue("15% reduced Haste Mana Reservation")]
            EnchantmentHasteManaReservation2, //Eternal Labyrinth

            [StringValue("10% reduced Hatred Mana Reservation")]
            EnchantmentHatredManaReservation1, //Merciless Labyrinth

            [StringValue("15% reduced Hatred Mana Reservation")]
            EnchantmentHatredManaReservation2, //Eternal Labyrinth

            [StringValue("10% increased Heavy Strike Attack Speed")]
            EnchantmentHeavyStrikeAttackSpeed1, //Merciless Labyrinth

            [StringValue("15% increased Heavy Strike Attack Speed")]
            EnchantmentHeavyStrikeAttackSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Heavy Strike Damage")]
            EnchantmentHeavyStrikeDamage1, //Merciless Labyrinth

            [StringValue("40% increased Heavy Strike Damage")]
            EnchantmentHeavyStrikeDamage2, //Eternal Labyrinth

            [StringValue("Heavy Strike has a 8% chance to deal Double Damage")]
            EnchantmentHeavyStrikeDoubleDamage1, //Merciless Labyrinth

            [StringValue("Heavy Strike has a 12% chance to deal Double Damage")]
            EnchantmentHeavyStrikeDoubleDamage2, //Eternal Labyrinth

            [StringValue("25% increased Herald of Ash Damage")]
            EnchantmentHeraldOfAshDamage1, //Merciless Labyrinth

            [StringValue("40% increased Herald of Ash Damage")]
            EnchantmentHeraldOfAshDamage2, //Eternal Labyrinth

            [StringValue("20% reduced Herald of Ash Mana Reservation")]
            EnchantmentHeraldOfAshManaReservation1, //Merciless Labyrinth

            [StringValue("30% reduced Herald of Ash Mana Reservation")]
            EnchantmentHeraldOfAshManaReservation2, //Eternal Labyrinth

            [StringValue("25% increased Herald of Ice Damage")]
            EnchantmentHeraldOfIceDamage1_, //Merciless Labyrinth

            [StringValue("40% increased Herald of Ice Damage")]
            EnchantmentHeraldOfIceDamage2, //Eternal Labyrinth

            [StringValue("20% reduced Herald of Ice Mana Reservation")]
            EnchantmentHeraldOfIceManaReservation1, //Merciless Labyrinth

            [StringValue("30% reduced Herald of Ice Mana Reservation")]
            EnchantmentHeraldOfIceManaReservation2, //Eternal Labyrinth

            [StringValue("25% increased Herald of Thunder Damage")]
            EnchantmentHeraldOfThunderDamage1, //Merciless Labyrinth

            [StringValue("40% increased Herald of Thunder Damage")]
            EnchantmentHeraldOfThunderDamage2_, //Eternal Labyrinth

            [StringValue("20% reduced Herald of Thunder Mana Reservation")]
            EnchantmentHeraldOfThunderManaReservation1, //Merciless Labyrinth

            [StringValue("30% reduced Herald of Thunder Mana Reservation")]
            EnchantmentHeraldOfThunderManaReservation2, //Eternal Labyrinth

            [StringValue("25% increased Ice Crash Damage")]
            EnchantmentIceCrashDamage1, //Merciless Labyrinth

            [StringValue("40% increased Ice Crash Damage")]
            EnchantmentIceCrashDamage2, //Eternal Labyrinth

            [StringValue("10% of Ice Crash Physical Damage gained as Extra Cold Damage")]
            EnchantmentIceCrashPhysicalDamagePercentToAddAsColdDamage1, //Merciless Labyrinth

            [StringValue("15% of Ice Crash Physical Damage gained as Extra Cold Damage")]
            EnchantmentIceCrashPhysicalDamagePercentToAddAsColdDamage2, //Eternal Labyrinth

            [StringValue("8% increased Ice Crash Area of Effect")]
            EnchantmentIceCrashRadius1, //Merciless Labyrinth

            [StringValue("12% increased Ice Crash Area of Effect")]
            EnchantmentIceCrashRadius2, //Eternal Labyrinth

            [StringValue("+24% to Ice Golem Elemental Resistances")]
            EnchantmentIceGolemElementalResistances1, //Merciless Labyrinth

            [StringValue("+36% to Ice Golem Elemental Resistances")]
            EnchantmentIceGolemElementalResistances2, //Eternal Labyrinth

            [StringValue("100% increased Effect of the Buff granted by your Ice Golems")]
            EnchantmentIceGolemGrantsPercentAdditionalCriticalStrikeChanceAndAccuracy1, //Merciless Labyrinth

            [StringValue("150% increased Effect of the Buff granted by your Ice Golems")]
            EnchantmentIceGolemGrantsPercentAdditionalCriticalStrikeChanceAndAccuracy2, //Eternal Labyrinth

            [StringValue("25% increased Ice Nova Damage")]
            EnchantmentIceNovaDamage1, //Merciless Labyrinth

            [StringValue("40% increased Ice Nova Damage")]
            EnchantmentIceNovaDamage2, //Eternal Labyrinth

            [StringValue("Ice Nova has +20% chance to Freeze")]
            EnchantmentIceNovaFreezeChance1, //Merciless Labyrinth

            [StringValue("Ice Nova has +30% chance to Freeze")]
            EnchantmentIceNovaFreezeChance2, //Eternal Labyrinth

            [StringValue("8% increased Ice Nova Area of Effect")]
            EnchantmentIceNovaRadius1, //Merciless Labyrinth

            [StringValue("12% increased Ice Nova Area of Effect")]
            EnchantmentIceNovaRadius2, //Eternal Labyrinth

            [StringValue("25% increased Ice Shot Damage")]
            EnchantmentIceShotDamage1, //Merciless Labyrinth

            [StringValue("40% increased Ice Shot Damage")]
            EnchantmentIceShotDamage2, //Eternal Labyrinth

            [StringValue("24% increased Ice Shot Duration")]
            EnchantmentIceShotDuration1, //Merciless Labyrinth

            [StringValue("36% increased Ice Shot Duration")]
            EnchantmentIceShotDuration2, //Eternal Labyrinth

            [StringValue("8% increased Ice Shot Area of Effect")]
            EnchantmentIceShotRadius1, //Merciless Labyrinth

            [StringValue("12% increased Ice Shot Area of Effect")]
            EnchantmentIceShotRadius2_, //Eternal Labyrinth

            [StringValue("25% increased Ice Spear Damage")]
            EnchantmentIceSpearDamage1, //Merciless Labyrinth

            [StringValue("40% increased Ice Spear Damage")]
            EnchantmentIceSpearDamage2, //Eternal Labyrinth

            [StringValue("10% Chance to gain a Power Charge on Critical Strike with Ice Spear")]
            EnchantmentIceSpearPercentChanceToGainPowerChargeOnCriticalStrike1, //Merciless Labyrinth

            [StringValue("15% Chance to gain a Power Charge on Critical Strike with Ice Spear")]
            EnchantmentIceSpearPercentChanceToGainPowerChargeOnCriticalStrike2_, //Eternal Labyrinth

            [StringValue("200% increased Ice Spear Critical Strike Chance in second form")]
            EnchantmentIceSpearSecondFormCriticalStrikeChance1, //Merciless Labyrinth

            [StringValue("300% increased Ice Spear Critical Strike Chance in second form")]
            EnchantmentIceSpearSecondFormCriticalStrikeChance2, //Eternal Labyrinth

            [StringValue("20% increased Ice Trap Cooldown Recovery Speed")]
            EnchantmentIceTrapCooldownSpeed1_, //Merciless Labyrinth

            [StringValue("30% increased Ice Trap Cooldown Recovery Speed")]
            EnchantmentIceTrapCooldownSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Ice Trap Damage")]
            EnchantmentIceTrapDamage1, //Merciless Labyrinth

            [StringValue("40% increased Ice Trap Damage")]
            EnchantmentIceTrapDamage2, //Eternal Labyrinth

            [StringValue("8% increased Ice Trap Area of Effect")]
            EnchantmentIceTrapRadius1_, //Merciless Labyrinth

            [StringValue("12% increased Ice Trap Area of Effect")]
            EnchantmentIceTrapRadius2, //Eternal Labyrinth

            [StringValue("34% increased Immortal Call Duration")]
            EnchantmentImmortalCallDuration1_, //Merciless Labyrinth

            [StringValue("36% increased Immortal Call Duration")]
            EnchantmentImmortalCallDuration2, //Eternal Labyrinth

            [StringValue("20% chance for Immortal Call to not consume Endurance Charges")]
            EnchantmentImmortalCallPercentChanceToNotConsumeEnduranceCharges1, //Merciless Labyrinth

            [StringValue("30% chance for Immortal Call to not consume Endurance Charges")]
            EnchantmentImmortalCallPercentChanceToNotConsumeEnduranceCharges2, //Eternal Labyrinth

            [StringValue("25% increased Incinerate Damage")]
            EnchantmentIncinerateDamage1, //Merciless Labyrinth

            [StringValue("40% increased Incinerate Damage")]
            EnchantmentIncinerateDamage2, //Eternal Labyrinth

            [StringValue("16% increased Incinerate Damage for each stage")]
            EnchantmentIncinerateDamagePerStage1_, //Merciless Labyrinth

            [StringValue("24% increased Incinerate Damage for each stage")]
            EnchantmentIncinerateDamagePerStage2, //Eternal Labyrinth

            [StringValue("20% increased Incinerate Projectile Speed")]
            EnchantmentIncinerateProjectileSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Incinerate Projectile Speed")]
            EnchantmentIncinerateProjectileSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Infernal Blow Damage")]
            EnchantmentInfernalBlowDamage1, //Merciless Labyrinth

            [StringValue("40% increased Infernal Blow Damage")]
            EnchantmentInfernalBlowDamage2, //Eternal Labyrinth

            [StringValue("10% of Infernal Blow Physical Damage gained as Extra Fire Damage")]
            EnchantmentInfernalBlowPhysicalDamagePercentToAddAsFireDamage1, //Merciless Labyrinth

            [StringValue("15% of Infernal Blow Physical Damage gained as Extra Fire Damage")]
            EnchantmentInfernalBlowPhysicalDamagePercentToAddAsFireDamage2_, //Eternal Labyrinth

            [StringValue("8% increased Infernal Blow Area of Effect")]
            EnchantmentInfernalBlowRadius1, //Merciless Labyrinth

            [StringValue("12% increased Infernal Blow Area of Effect")]
            EnchantmentInfernalBlowRadius2, //Eternal Labyrinth

            [StringValue("25% increased Kinetic Blast Damage")]
            EnchantmentKineticBlastDamage1, //Merciless Labyrinth

            [StringValue("40% increased Kinetic Blast Damage")]
            EnchantmentKineticBlastDamage2, //Eternal Labyrinth

            [StringValue("Kinetic Blast has a 50% chance for an additional explosion")]
            EnchantmentKineticBlastExplosions1_, //Merciless Labyrinth

            [StringValue("Kinetic Blast has a 75% chance for an additional explosion")]
            EnchantmentKineticBlastExplosions2, //Eternal Labyrinth

            [StringValue("8% increased Kinetic Blast Area of Effect")]
            EnchantmentKineticBlastRadius1, //Merciless Labyrinth

            [StringValue("12% increased Kinetic Blast Area of Effect")]
            EnchantmentKineticBlastRadius2, //Eternal Labyrinth

            [StringValue("10% increased Leap Slam Attack Speed")]
            EnchantmentLeapSlamAttackSpeed1, //Merciless Labyrinth

            [StringValue("15% increased Leap Slam Attack Speed")]
            EnchantmentLeapSlamAttackSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Leap Slam Damage")]
            EnchantmentLeapSlamDamage1, //Merciless Labyrinth

            [StringValue("40% increased Leap Slam Damage")]
            EnchantmentLeapSlamDamage2, //Eternal Labyrinth

            [StringValue("8% increased Leap Slam Area of Effect")]
            EnchantmentLeapSlamRadius1, //Merciless Labyrinth

            [StringValue("12% increased Leap Slam Area of Effect")]
            EnchantmentLeapSlamRadius2, //Eternal Labyrinth

            [StringValue("25% increased Lightning Arrow Damage")]
            EnchantmentLightningArrowDamage1, //Merciless Labyrinth

            [StringValue("40% increased Lightning Arrow Damage")]
            EnchantmentLightningArrowDamage2, //Eternal Labyrinth

            [StringValue("Lightning Arrow hits an additional Enemy")]
            EnchantmentLightningArrowExtraTargets1, //Merciless Labyrinth

            [StringValue("Lightning Arrow hits 2 additional Enemies")]
            EnchantmentLightningArrowExtraTargets2, //Eternal Labyrinth

            [StringValue("8% increased Lightning Arrow Area of Effect")]
            EnchantmentLightningArrowRadius1, //Merciless Labyrinth

            [StringValue("12% increased Lightning Arrow Area of Effect")]
            EnchantmentLightningArrowRadius2, //Eternal Labyrinth

            [StringValue("+24% to Lightning Golem Elemental Resistances")]
            EnchantmentLightningGolemElementalResistances1, //Merciless Labyrinth

            [StringValue("+36% to Lightning Golem Elemental Resistances")]
            EnchantmentLightningGolemElementalResistances2, //Eternal Labyrinth

            [StringValue("100% increased Effect of the Buff granted by your Lightning Golems")]
            EnchantmentLightningGolemGrantedBuffEffect1, //Merciless Labyrinth

            [StringValue("150% increased Effect of the Buff granted by your Lightning Golems")]
            EnchantmentLightningGolemGrantedBuffEffect2_, //Eternal Labyrinth

            [StringValue("Lightning Strike pierces 2 additional Targets")]
            EnchantmentLightningStrikeAdditionalPierce1, //Merciless Labyrinth

            [StringValue("Lightning Strike pierces 3 additional Targets")]
            EnchantmentLightningStrikeAdditionalPierce2, //Eternal Labyrinth

            [StringValue("25% increased Lightning Strike Damage")]
            EnchantmentLightningStrikeDamage1__, //Merciless Labyrinth

            [StringValue("40% increased Lightning Strike Damage")]
            EnchantmentLightningStrikeDamage2, //Eternal Labyrinth

            [StringValue("Lightning Strike fires 2 additional Projectiles")]
            EnchantmentLightningStrikeNumOfAdditionalProjectiles1, //Merciless Labyrinth

            [StringValue("Lightning Strike fires 3 additional Projectiles")]
            EnchantmentLightningStrikeNumOfAdditionalProjectiles2, //Eternal Labyrinth

            [StringValue("40% increased Lightning Tendrils Critical Strike Chance")]
            EnchantmentLightningTendrilsCriticalStrikeChance1, //Merciless Labyrinth

            [StringValue("60% increased Lightning Tendrils Critical Strike Chance")]
            EnchantmentLightningTendrilsCriticalStrikeChance2, //Eternal Labyrinth

            [StringValue("25% increased Lightning Tendrils Damage")]
            EnchantmentLightningTendrilsDamage1, //Merciless Labyrinth

            [StringValue("40% increased Lightning Tendrils Damage")]
            EnchantmentLightningTendrilsDamage2, //Eternal Labyrinth

            [StringValue("8% increased Lightning Tendrils Area of Effect")]
            EnchantmentLightningTendrilsRadius1_, //Merciless Labyrinth

            [StringValue("12% increased Lightning Tendrils Area of Effect")]
            EnchantmentLightningTendrilsRadius2, //Eternal Labyrinth

            [StringValue("Lightning Trap pierces 2 additional Targets")]
            EnchantmentLightningTrapAdditionalPierce1, //Merciless Labyrinth

            [StringValue("Lightning Trap pierces 3 additional Targets")]
            EnchantmentLightningTrapAdditionalPierce2, //Eternal Labyrinth

            [StringValue("20% increased Lightning Trap Cooldown Recovery Speed")]
            EnchantmentLightningTrapCooldownSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Lightning Trap Cooldown Recovery Speed")]
            EnchantmentLightningTrapCooldownSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Lightning Trap Damage")]
            EnchantmentLightningTrapDamage1, //Merciless Labyrinth

            [StringValue("40% increased Lightning Trap Damage")]
            EnchantmentLightningTrapDamage2, //Eternal Labyrinth

            [StringValue("8% increased Lightning Warp Cast Speed")]
            EnchantmentLightningWarpCastSpeed1, //Merciless Labyrinth

            [StringValue("12% increased Lightning Warp Cast Speed")]
            EnchantmentLightningWarpCastSpeed2_, //Eternal Labyrinth

            [StringValue("25% increased Lightning Warp Damage")]
            EnchantmentLightningWarpDamage1, //Merciless Labyrinth

            [StringValue("40% increased Lightning Warp Damage")]
            EnchantmentLightningWarpDamage2, //Eternal Labyrinth

            [StringValue("20% reduced Lightning Warp Duration")]
            EnchantmentLightningWarpDuration1, //Merciless Labyrinth

            [StringValue("30% reduced Lightning Warp Duration")]
            EnchantmentLightningWarpDuration2, //Eternal Labyrinth

            [StringValue("25% increased Magma Orb Damage")]
            EnchantmentMagmaOrbDamage1, //Merciless Labyrinth

            [StringValue("40% increased Magma Orb Damage")]
            EnchantmentMagmaOrbDamage2, //Eternal Labyrinth

            [StringValue("Magma Orb Chains an additional time")]
            EnchantmentMagmaOrbNumOfAdditionalProjectilesInChain1, //Merciless Labyrinth

            [StringValue("Magma Orb Chains an additional 2 times")]
            EnchantmentMagmaOrbNumOfAdditionalProjectilesInChain2, //Eternal Labyrinth

            [StringValue("8% increased Magma Orb Area of Effect")]
            EnchantmentMagmaOrbRadius1, //Merciless Labyrinth

            [StringValue("12% increased Magma Orb Area of Effect")]
            EnchantmentMagmaOrbRadius2, //Eternal Labyrinth

            [StringValue("Mirror Arrow and Mirror Arrow Clones have 10% increased Attack Speed")]
            EnchantmentMirrorArrowAttackSpeed1, //Merciless Labyrinth

            [StringValue("Mirror Arrow and Mirror Arrow Clones have 15% increased Attack Speed")]
            EnchantmentMirrorArrowAttackSpeed2, //Eternal Labyrinth

            [StringValue("20% increased Mirror Arrow Cooldown Recovery Speed")]
            EnchantmentMirrorArrowCooldownSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Mirror Arrow Cooldown Recovery Speed")]
            EnchantmentMirrorArrowCooldownSpeed2, //Eternal Labyrinth

            [StringValue("Mirror Arrow and Mirror Arrow Clones deal 25% increased Damage")]
            EnchantmentMirrorArrowDamage1, //Merciless Labyrinth

            [StringValue("Mirror Arrow and Mirror Arrow Clones deal 40% increased Damage")]
            EnchantmentMirrorArrowDamage2, //Eternal Labyrinth

            [StringValue("100% increased Molten Shell Buff Effect")]
            EnchantmentMoltenShellArmour1, //Merciless Labyrinth

            [StringValue("150% increased Molten Shell Buff Effect")]
            EnchantmentMoltenShellArmour2, //Eternal Labyrinth

            [StringValue("25% increased Molten Shell Damage")]
            EnchantmentMoltenShellDamage1, //Merciless Labyrinth

            [StringValue("40% increased Molten Shell Damage")]
            EnchantmentMoltenShellDamage2, //Eternal Labyrinth

            [StringValue("25% increased Molten Strike Damage")]
            EnchantmentMoltenStrikeDamage1, //Merciless Labyrinth

            [StringValue("40% increased Molten Strike Damage")]
            EnchantmentMoltenStrikeDamage2, //Eternal Labyrinth

            [StringValue("Molten Strike fires 2 additional Projectiles")]
            EnchantmentMoltenStrikeNumOfAdditionalProjectiles1_, //Merciless Labyrinth

            [StringValue("Molten Strike fires 3 additional Projectiles")]
            EnchantmentMoltenStrikeNumOfAdditionalProjectiles2, //Eternal Labyrinth

            [StringValue("8% increased Molten Strike Area of Effect")]
            EnchantmentMoltenStrikeRadius1, //Merciless Labyrinth

            [StringValue("12% increased Molten Strike Area of Effect")]
            EnchantmentMoltenStrikeRadius2, //Eternal Labyrinth

            [StringValue("25% increased Orb of Storms Damage")]
            EnchantmentOrbOfStormsDamage1, //Merciless Labyrinth

            [StringValue("40% increased Orb of Storms Damage")]
            EnchantmentOrbOfStormsDamage2, //Eternal Labyrinth

            [StringValue("24% increased Phase Run Duration")]
            EnchantmentPhaseRunDuration1, //Merciless Labyrinth

            [StringValue("36% increased Phase Run Duration")]
            EnchantmentPhaseRunDuration2, //Eternal Labyrinth

            [StringValue("20% chance for Phase Run to not consume Frenzy Charges")]
            EnchantmentPhaseRunPercentChanceToNotConsumeFrenzyCharges1, //Merciless Labyrinth

            [StringValue("30% chance for Phase Run to not consume Frenzy Charges")]
            EnchantmentPhaseRunPercentChanceToNotConsumeFrenzyCharges2, //Eternal Labyrinth

            [StringValue("20% increased Poacher's Mark Curse Effect")]
            EnchantmentPoachersMarkCurseEffect1, //Merciless Labyrinth

            [StringValue("30% increased Poacher's Mark Curse Effect")]
            EnchantmentPoachersMarkCurseEffect2, //Eternal Labyrinth

            [StringValue("30% increased Poacher's Mark Duration")]
            EnchantmentPoachersMarkDuration1, //Merciless Labyrinth

            [StringValue("45% increased Poacher's Mark Duration")]
            EnchantmentPoachersMarkDuration2, //Eternal Labyrinth

            [StringValue("10% increased Power Siphon Attack Speed")]
            EnchantmentPowerSiphonAttackSpeed1_, //Merciless Labyrinth

            [StringValue("15% increased Power Siphon Attack Speed")]
            EnchantmentPowerSiphonAttackSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Power Siphon Damage")]
            EnchantmentPowerSiphonDamage1, //Merciless Labyrinth

            [StringValue("40% increased Power Siphon Damage")]
            EnchantmentPowerSiphonDamage2, //Eternal Labyrinth

            [StringValue("30% Chance to gain an additional Power Charge on Kill with Power Siphon")]
            EnchantmentPowerSiphonPercentChanceToGainPowerChargeOnKill1__, //Merciless Labyrinth

            [StringValue("45% Chance to gain an additional Power Charge on Kill with Power Siphon")]
            EnchantmentPowerSiphonPercentChanceToGainPowerChargeOnKill2, //Eternal Labyrinth

            [StringValue("20% increased Projectile Weakness Curse Effect")]
            EnchantmentProjectileWeaknessCurseEffect1, //Merciless Labyrinth

            [StringValue("30% increased Projectile Weakness Curse Effect")]
            EnchantmentProjectileWeaknessCurseEffect2, //Eternal Labyrinth

            [StringValue("30% increased Projectile Weakness Duration")]
            EnchantmentProjectileWeaknessDuration1, //Merciless Labyrinth

            [StringValue("45% increased Projectile Weakness Duration")]
            EnchantmentProjectileWeaknessDuration2_, //Eternal Labyrinth

            [StringValue("25% increased Puncture Damage")]
            EnchantmentPunctureDamage1, //Merciless Labyrinth

            [StringValue("40% increased Puncture Damage")]
            EnchantmentPunctureDamage2, //Eternal Labyrinth

            [StringValue("30% increased Puncture Duration")]
            EnchantmentPunctureDuration1, //Merciless Labyrinth

            [StringValue("45% increased Puncture Duration")]
            EnchantmentPunctureDuration2, //Eternal Labyrinth

            [StringValue("20% Chance for Puncture to Maim on hit")]
            EnchantmentPunctureMaimOnHitPercentChance1_, //Merciless Labyrinth

            [StringValue("30% Chance for Puncture to Maim on hit")]
            EnchantmentPunctureMaimOnHitPercentChance2, //Eternal Labyrinth

            [StringValue("20% increased Punishment Curse Effect")]
            EnchantmentPunishmentCurseEffect1, //Merciless Labyrinth

            [StringValue("30% increased Punishment Curse Effect")]
            EnchantmentPunishmentCurseEffect2, //Eternal Labyrinth

            [StringValue("30% increased Punishment Duration")]
            EnchantmentPunishmentDuration1, //Merciless Labyrinth

            [StringValue("45% increased Punishment Duration")]
            EnchantmentPunishmentDuration2, //Eternal Labyrinth

            [StringValue("14% reduced Purity of Elements Mana Reservation")]
            EnchantmentPurityOfElementsManaReservation1_, //Merciless Labyrinth

            [StringValue("20% reduced Purity of Elements Mana Reservation")]
            EnchantmentPurityOfElementsManaReservation2, //Eternal Labyrinth

            [StringValue("14% reduced Purity of Fire Mana Reservation")]
            EnchantmentPurityOfFireManaReservation1, //Merciless Labyrinth

            [StringValue("20% reduced Purity of Fire Mana Reservation")]
            EnchantmentPurityOfFireManaReservation2, //Eternal Labyrinth

            [StringValue("14% reduced Purity of Ice Mana Reservation")]
            EnchantmentPurityOfIceManaReservation1, //Merciless Labyrinth

            [StringValue("20% reduced Purity of Ice Mana Reservation")]
            EnchantmentPurityOfIceManaReservation2, //Eternal Labyrinth

            [StringValue("14% reduced Purity of Lightning Mana Reservation")]
            EnchantmentPurityOfLightningManaReservation1, //Merciless Labyrinth

            [StringValue("20% reduced Purity of Lightning Mana Reservation")]
            EnchantmentPurityOfLightningManaReservation2, //Eternal Labyrinth

            [StringValue("10% increased Rain of Arrows Attack Speed")]
            EnchantmentRainOfArrowsAttackSpeed1, //Merciless Labyrinth

            [StringValue("15% increased Rain of Arrows Attack Speed")]
            EnchantmentRainOfArrowsAttackSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Rain of Arrows Damage")]
            EnchantmentRainOfArrowsDamage1, //Merciless Labyrinth

            [StringValue("40% increased Rain of Arrows Damage")]
            EnchantmentRainOfArrowsDamage2, //Eternal Labyrinth

            [StringValue("8% increased Rain of Arrows Area of Effect")]
            EnchantmentRainOfArrowsRadius1, //Merciless Labyrinth

            [StringValue("12% increased Rain of Arrows Area of Effect")]
            EnchantmentRainOfArrowsRadius2, //Eternal Labyrinth

            [StringValue("Spectres have 25% increased Damage")]
            EnchantmentRaiseSpectreDamage1, //Merciless Labyrinth

            [StringValue("Spectres have 40% increased Damage")]
            EnchantmentRaiseSpectreDamage2, //Eternal Labyrinth

            [StringValue("Zombies deal 25% increased Damage")]
            EnchantmentRaiseZombieDamage1, //Merciless Labyrinth

            [StringValue("Zombies deal 40% increased Damage")]
            EnchantmentRaiseZombieDamage2, //Eternal Labyrinth

            [StringValue("50% increased Rallying Cry Buff Effect")]
            EnchantmentRallyingCryBuffEffect1, //Merciless Labyrinth

            [StringValue("75% increased Rallying Cry Buff Effect")]
            EnchantmentRallyingCryBuffEffect2, //Eternal Labyrinth

            [StringValue("30% increased Rallying Cry Duration")]
            EnchantmentRallyingCryDuration1, //Merciless Labyrinth

            [StringValue("45% increased Rallying Cry Duration")]
            EnchantmentRallyingCryDuration2,                                //Eternal Labyrinth
            [StringValue("25% increased Reave Damage")] EnchantmentReaveDamage1, //Merciless Labyrinth
            [StringValue("40% increased Reave Damage")] EnchantmentReaveDamage2, //Eternal Labyrinth
            [StringValue("8% increased Reave Radius")] EnchantmentReaveRadius1,  //Merciless Labyrinth
            [StringValue("12% increased Reave Radius")] EnchantmentReaveRadius2, //Eternal Labyrinth

            [StringValue("20% increased Reckoning Cooldown Recovery Speed")]
            EnchantmentReckoningCooldownSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Reckoning Cooldown Recovery Speed")]
            EnchantmentReckoningCooldownSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Reckoning Damage")]
            EnchantmentReckoningDamage1, //Merciless Labyrinth

            [StringValue("40% increased Reckoning Damage")]
            EnchantmentReckoningDamage2, //Eternal Labyrinth

            [StringValue("30% increased Rejuvenation Totem Aura Effect")]
            EnchantmentRejuvinationTotemLifeRegeneration1, //Merciless Labyrinth

            [StringValue("45% increased Rejuvenation Totem Aura Effect")]
            EnchantmentRejuvinationTotemLifeRegeneration2, //Eternal Labyrinth

            [StringValue("Gain 10% of Rejuvenation Totem Life Regeneration as extra Mana Regeneration")]
            EnchantmentRejuvinationTotemPercentLifeRegenerationAddedAsManaRegeneration1_, //Merciless Labyrinth

            [StringValue("Gain 15% of Rejuvenation Totem Life Regeneration as extra Mana Regeneration")]
            EnchantmentRejuvinationTotemPercentLifeRegenerationAddedAsManaRegeneration2_, //Eternal Labyrinth

            [StringValue("25% increased Righteous Fire Damage")]
            EnchantmentRighteousFireDamage1, //Merciless Labyrinth

            [StringValue("40% increased Righteous Fire Damage")]
            EnchantmentRighteousFireDamage2, //Eternal Labyrinth

            [StringValue("8% increased Righteous Fire Area of Effect")]
            EnchantmentRighteousFireRadius1, //Merciless Labyrinth

            [StringValue("12% increased Righteous Fire Area of Effect")]
            EnchantmentRighteousFireRadius2, //Eternal Labyrinth

            [StringValue("Righteous Fire grants 20% increased Spell Damage")]
            EnchantmentRighteousFireSpellDamage1, //Merciless Labyrinth

            [StringValue("Righteous Fire grants 30% increased Spell Damage")]
            EnchantmentRighteousFireSpellDamage2, //Eternal Labyrinth

            [StringValue("20% increased Riposte Cooldown Recovery Speed")]
            EnchantmentRiposteCooldownSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Riposte Cooldown Recovery Speed")]
            EnchantmentRiposteCooldownSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Riposte Damage")]
            EnchantmentRiposteDamage1, //Merciless Labyrinth

            [StringValue("40% increased Riposte Damage")]
            EnchantmentRiposteDamage2, //Eternal Labyrinth

            [StringValue("25% increased Searing Bond Damage")]
            EnchantmentSearingBondDamage1, //Merciless Labyrinth

            [StringValue("40% increased Searing Bond Damage")]
            EnchantmentSearingBondDamage2, //Eternal Labyrinth

            [StringValue("24% increased Searing Bond Totem Elemental Resistances")]
            EnchantmentSearingBondTotemElementalResistances1, //Merciless Labyrinth

            [StringValue("36% increased Searing Bond Totem Elemental Resistances")]
            EnchantmentSearingBondTotemElementalResistances2, //Eternal Labyrinth

            [StringValue("40% increased Searing Bond Totem Placement Speed")]
            EnchantmentSearingBondTotemPlacementSpeed1, //Merciless Labyrinth

            [StringValue("60% increased Searing Bond Totem Placement Speed")]
            EnchantmentSearingBondTotemPlacementSpeed2_, //Eternal Labyrinth

            [StringValue("10% increased Shield Charge Attack Speed")]
            EnchantmentShieldChargeAttackSpeed1, //Merciless Labyrinth

            [StringValue("15% increased Shield Charge Attack Speed")]
            EnchantmentShieldChargeAttackSpeed2_, //Eternal Labyrinth

            [StringValue("25% increased Shield Charge Damage")]
            EnchantmentShieldChargeDamage1, //Merciless Labyrinth

            [StringValue("40% increased Shield Charge Damage")]
            EnchantmentShieldChargeDamage2, //Eternal Labyrinth

            [StringValue("25% increased Shock Nova Damage")]
            EnchantmentShockNovaDamage1, //Merciless Labyrinth

            [StringValue("40% increased Shock Nova Damage")]
            EnchantmentShockNovaDamage2_, //Eternal Labyrinth

            [StringValue("Shock Nova ring deals 40% increased Damage")]
            EnchantmentShockNovaLargerRingDamage1, //Merciless Labyrinth

            [StringValue("Shock Nova ring deals 60% increased Damage")]
            EnchantmentShockNovaLargerRingDamage2, //Eternal Labyrinth

            [StringValue("8% increased Shock Nova Area of Effect")]
            EnchantmentShockNovaRadius1, //Merciless Labyrinth

            [StringValue("12% increased Shock Nova Area of Effect")]
            EnchantmentShockNovaRadius2_, //Eternal Labyrinth

            [StringValue("10% increased Shockwave Totem Cast Speed")]
            EnchantmentShockwaveTotemCastSpeed1_, //Merciless Labyrinth

            [StringValue("15% increased Shockwave Totem Cast Speed")]
            EnchantmentShockwaveTotemCastSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Shockwave Totem Damage")]
            EnchantmentShockwaveTotemDamage1, //Merciless Labyrinth

            [StringValue("40% increased Shockwave Totem Damage")]
            EnchantmentShockwaveTotemDamage2, //Eternal Labyrinth

            [StringValue("8% increased Shockwave Totem Area of Effect")]
            EnchantmentShockwaveTotemRadius1, //Merciless Labyrinth

            [StringValue("12% increased Shockwave Totem Area of Effect")]
            EnchantmentShockwaveTotemRadius2, //Eternal Labyrinth

            [StringValue("25% increased Shrapnel Shot Damage")]
            EnchantmentShrapnelShotDamage1, //Merciless Labyrinth

            [StringValue("40% increased Shrapnel Shot Damage")]
            EnchantmentShrapnelShotDamage2, //Eternal Labyrinth

            [StringValue("10% of Shrapnel Shot Physical Damage gained as extra Lightning Damage")]
            EnchantmentShrapnelShotPhysicalDamagePercentToAddAsLightningDamage1_, //Merciless Labyrinth

            [StringValue("15% of Shrapnel Shot Physical Damage gained as extra Lightning Damage")]
            EnchantmentShrapnelShotPhysicalDamagePercentToAddAsLightningDamage2, //Eternal Labyrinth

            [StringValue("8% increased Shrapnel Shot Area of Effect")]
            EnchantmentShrapnelShotRadius1, //Merciless Labyrinth

            [StringValue("12% increased Shrapnel Shot Area of Effect")]
            EnchantmentShrapnelShotRadius2, //Eternal Labyrinth

            [StringValue("10% increased Siege Ballista Attack Speed")]
            EnchantmentSiegeBallistaAttackSpeed1, //Merciless Labyrinth

            [StringValue("15% increased Siege Ballista Attack Speed")]
            EnchantmentSiegeBallistaAttackSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Siege Ballista Damage")]
            EnchantmentSiegeBallistaDamage1, //Merciless Labyrinth

            [StringValue("40% increased Siege Ballista Damage")]
            EnchantmentSiegeBallistaDamage2, //Eternal Labyrinth

            [StringValue("30% increased Siege Ballista Totem Placement Speed")]
            EnchantmentSiegeBallistaTotemPlacementSpeed1, //Merciless Labyrinth

            [StringValue("45% increased Siege Ballista Totem Placement Speed")]
            EnchantmentSiegeBallistaTotemPlacementSpeed2, //Eternal Labyrinth

            [StringValue("8% increased Dark Pact Area of Effect")]
            EnchantmentSkeletalChainsAreaOfEffect1_, //Merciless Labyrinth

            [StringValue("12% increased Dark Pact Area of Effect")]
            EnchantmentSkeletalChainsAreaOfEffect2, //Eternal Labyrinth

            [StringValue("8% increased Dark Pact Cast Speed")]
            EnchantmentSkeletalChainsCastSpeed1, //Merciless Labyrinth

            [StringValue("12% increased Dark Pact Cast Speed")]
            EnchantmentSkeletalChainsCastSpeed2_, //Eternal Labyrinth

            [StringValue("25% increased Dark Pact Damage")]
            EnchantmentSkeletalChainsDamage1, //Merciless Labyrinth

            [StringValue("40% increased Dark Pact Damage")]
            EnchantmentSkeletalChainsDamage2, //Eternal Labyrinth

            [StringValue("20% increased Smoke Mine Duration")]
            EnchantmentSmokeMineDuration1, //Merciless Labyrinth

            [StringValue("30% increased Smoke Mine Duration")]
            EnchantmentSmokeMineDuration2, //Eternal Labyrinth

            [StringValue("Smoke Mine grants additional 20% increased Movement Speed")]
            EnchantmentSmokeMineMovementSpeed1, //Merciless Labyrinth

            [StringValue("Smoke Mine grants additional 30% increased Movement Speed")]
            EnchantmentSmokeMineMovementSpeed2_, //Eternal Labyrinth

            [StringValue("25% increased Spark Damage")]
            EnchantmentSparkDamage1, //Merciless Labyrinth

            [StringValue("40% increased Spark Damage")]
            EnchantmentSparkDamage2, //Eternal Labyrinth

            [StringValue("Spark fires 2 additional Projectiles")]
            EnchantmentSparkNumOfAdditionalProjectiles1, //Merciless Labyrinth

            [StringValue("Spark fires 3 additional Projectiles")]
            EnchantmentSparkNumOfAdditionalProjectiles2, //Eternal Labyrinth

            [StringValue("20% increased Spark Projectile Speed")]
            EnchantmentSparkProjectileSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Spark Projectile Speed")]
            EnchantmentSparkProjectileSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Spectral Shield Throw Damage")]
            EnchantmentSpectralShieldThrowDamage1, //Merciless Labyrinth

            [StringValue("40% increased Spectral Shield Throw Damage")]
            EnchantmentSpectralShieldThrowDamage2, //Eternal Labyrinth

            [StringValue("Spectral Shield Throw fires 3 additional Shard Projectiles")]
            EnchantmentSpectralShieldThrowNumOfAdditionalProjectiles1, //Merciless Labyrinth

            [StringValue("Spectral Shield Throw fires 5 additional Shard Projectiles")]
            EnchantmentSpectralShieldThrowNumOfAdditionalProjectiles2, //Eternal Labyrinth

            [StringValue("20% increased Spectral Shield Throw Projectile Speed")]
            EnchantmentSpectralShieldThrowProjectileSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Spectral Shield Throw Projectile Speed")]
            EnchantmentSpectralShieldThrowProjectileSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Spectral Throw Damage")]
            EnchantmentSpectralThrowDamage1, //Merciless Labyrinth

            [StringValue("40% increased Spectral Throw Damage")]
            EnchantmentSpectralThrowDamage2, //Eternal Labyrinth

            [StringValue("20% reduced Spectral Throw Projectile Deceleration")]
            EnchantmentSpectralThrowProjectileDeceleration1, //Merciless Labyrinth

            [StringValue("30% reduced Spectral Throw Projectile Deceleration")]
            EnchantmentSpectralThrowProjectileDeceleration2, //Eternal Labyrinth

            [StringValue("20% increased Spectral Throw Projectile Speed")]
            EnchantmentSpectralThrowProjectileSpeed1___, //Merciless Labyrinth

            [StringValue("30% increased Spectral Throw Projectile Speed")]
            EnchantmentSpectralThrowProjectileSpeed2, //Eternal Labyrinth

            [StringValue("Spectres have 8% increased Attack and Cast Speed")]
            EnchantmentSpectreAttackAndCastSpeed1, //Merciless Labyrinth

            [StringValue("Spectres have 12% increased Attack and Cast Speed")]
            EnchantmentSpectreAttackAndCastSpeed2, //Eternal Labyrinth

            [StringValue("+24% to Raised Spectre Elemental Resistances")]
            EnchantmentSpectreElementalResistances1, //Merciless Labyrinth

            [StringValue("+36% to Raised Spectre Elemental Resistances")]
            EnchantmentSpectreElementalResistances2, //Eternal Labyrinth

            [StringValue("30% increased Spirit Offering Duration")]
            EnchantmentSpiritOfferingDuration1, //Merciless Labyrinth

            [StringValue("45% increased Spirit Offering Duration")]
            EnchantmentSpiritOfferingDuration2, //Eternal Labyrinth

            [StringValue("Spirit Offering grants +8% of Physical Damage as Extra Chaos Damage")]
            EnchantmentSpiritOfferingPhysicalAddedAsChaos1_, //Merciless Labyrinth

            [StringValue("Spirit Offering grants +12% of Physical Damage as Extra Chaos Damage")]
            EnchantmentSpiritOfferingPhysicalAddedAsChaos2_, //Eternal Labyrinth

            [StringValue("60% increased Split Arrow Critical Strike Chance")]
            EnchantmentSplitArrowCriticalStrikeChance1, //Merciless Labyrinth

            [StringValue("90% increased Split Arrow Critical Strike Chance")]
            EnchantmentSplitArrowCriticalStrikeChance2, //Eternal Labyrinth

            [StringValue("25% increased Split Arrow Damage")]
            EnchantmentSplitArrowDamage1, //Merciless Labyrinth

            [StringValue("40% increased Split Arrow Damage")]
            EnchantmentSplitArrowDamage2, //Eternal Labyrinth

            [StringValue("Split Arrow fires 2 additional Projectiles")]
            EnchantmentSplitArrowNumOfAdditionalProjectiles1, //Merciless Labyrinth

            [StringValue("Split Arrow fires 3 additional Projectiles")]
            EnchantmentSplitArrowNumOfAdditionalProjectiles2, //Eternal Labyrinth

            [StringValue("25% increased Static Strike Damage")]
            EnchantmentStaticStrikeDamage1, //Merciless Labyrinth

            [StringValue("40% increased Static Strike Damage")]
            EnchantmentStaticStrikeDamage2, //Eternal Labyrinth

            [StringValue("30% reduced Static Strike Duration")]
            EnchantmentStaticStrikeDuration1, //Merciless Labyrinth

            [StringValue("45% reduced Static Strike Duration")]
            EnchantmentStaticStrikeDuration2, //Eternal Labyrinth

            [StringValue("8% increased Static Strike Area of Effect")]
            EnchantmentStaticStrikeRadius1_, //Merciless Labyrinth

            [StringValue("12% increased Static Strike Area of Effect")]
            EnchantmentStaticStrikeRadius2, //Eternal Labyrinth

            [StringValue("+24% to Stone Golem Elemental Resistances")]
            EnchantmentStoneGolemElementalResistances1___, //Merciless Labyrinth

            [StringValue("+36% to Stone Golem Elemental Resistances")]
            EnchantmentStoneGolemElementalResistances2, //Eternal Labyrinth

            [StringValue("100% increased Effect of the Buff granted by your Stone Golems")]
            EnchantmentStoneGolemGrantedBuffEffect1, //Merciless Labyrinth

            [StringValue("150% increased Effect of the Buff granted by your Stone Golems")]
            EnchantmentStoneGolemGrantedBuffEffect2, //Eternal Labyrinth

            [StringValue("30% chance to Avoid interruption from Stuns while Casting Storm Burst")]
            EnchantmentStormBurstAvoidStunWhileCasting1, //Merciless Labyrinth

            [StringValue("45% chance to Avoid interruption from Stuns while Casting Storm Burst")]
            EnchantmentStormBurstAvoidStunWhileCasting2, //Eternal Labyrinth

            [StringValue("25% increased Storm Burst Damage")]
            EnchantmentStormBurstDamage1, //Merciless Labyrinth

            [StringValue("40% increased Storm Burst Damage")]
            EnchantmentStormBurstDamage2_, //Eternal Labyrinth

            [StringValue("8% increased Storm Burst Area of Effect")]
            EnchantmentStormBurstRadius1, //Merciless Labyrinth

            [StringValue("12% increased Storm Burst Area of Effect")]
            EnchantmentStormBurstRadius2, //Eternal Labyrinth

            [StringValue("25% increased Storm Call Damage")]
            EnchantmentStormCallDamage1, //Merciless Labyrinth

            [StringValue("40% increased Storm Call Damage")]
            EnchantmentStormCallDamage2, //Eternal Labyrinth

            [StringValue("20% reduced Storm Call Duration")]
            EnchantmentStormCallDuration1, //Merciless Labyrinth

            [StringValue("30% reduced Storm Call Duration")]
            EnchantmentStormCallDuration2, //Eternal Labyrinth

            [StringValue("8% increased Storm Call Area of Effect")]
            EnchantmentStormCallRadius1, //Merciless Labyrinth

            [StringValue("12% increased Storm Call Area of Effect")]
            EnchantmentStormCallRadius2, //Eternal Labyrinth

            [StringValue("60% increased Orb of Storms Critical Strike Chance")]
            EnchantmentStormCloudCriticalStrikeChance1, //Merciless Labyrinth

            [StringValue("90% increased Orb of Storms Critical Strike Chance")]
            EnchantmentStormCloudCriticalStrikeChance2, //Eternal Labyrinth

            [StringValue("8% increased Orb of Storms Area of Effect")]
            EnchantmentStormCloudRadius1, //Merciless Labyrinth

            [StringValue("12% increased Orb of Storms Area of Effect")]
            EnchantmentStormCloudRadius2, //Eternal Labyrinth

            [StringValue("Summon Raging Spirit has 12% chance to summon an extra Minion")]
            EnchantmentSummonedRagingSpiritChanceToSpawnAdditionalMinion1, //Merciless Labyrinth

            [StringValue("Summon Raging Spirit has 18% chance to summon an extra Minion")]
            EnchantmentSummonedRagingSpiritChanceToSpawnAdditionalMinion2, //Eternal Labyrinth

            [StringValue("20% increased Summon Raging Spirit Duration")]
            EnchantmentSummonedRagingSpiritDuration1, //Merciless Labyrinth

            [StringValue("30% increased Summon Raging Spirit Duration")]
            EnchantmentSummonedRagingSpiritDuration2, //Eternal Labyrinth

            [StringValue("Flame Golems have 25% increased Damage")]
            EnchantmentSummonFlameGolemDamage1, //Merciless Labyrinth

            [StringValue("Flame Golems have 40% increased Damage")]
            EnchantmentSummonFlameGolemDamage2, //Eternal Labyrinth

            [StringValue("Ice Golems deal 25% increased Damage")]
            EnchantmentSummonIceGolemDamage1, //Merciless Labyrinth

            [StringValue("Ice Golems deal 40% increased Damage")]
            EnchantmentSummonIceGolemDamage2, //Eternal Labyrinth

            [StringValue("Lightning Golems deal 25% increased Damage")]
            EnchantmentSummonLightningGolemDamage1__, //Merciless Labyrinth

            [StringValue("Lightning Golems deal 40% increased Damage")]
            EnchantmentSummonLightningGolemDamage2_, //Eternal Labyrinth

            [StringValue("Raging Spirits have 25% increased Damage")]
            EnchantmentSummonRagingSpiritDamage1, //Merciless Labyrinth

            [StringValue("Raging Spirits have 40% increased Damage")]
            EnchantmentSummonRagingSpiritDamage2_, //Eternal Labyrinth

            [StringValue("Skeletons deal 25% increased Damage")]
            EnchantmentSummonSkeletonsDamage1, //Merciless Labyrinth

            [StringValue("Skeletons deal 40% increased Damage")]
            EnchantmentSummonSkeletonsDamage2, //Eternal Labyrinth

            [StringValue("20% chance to Summon an additional Skeleton Warrior with Summon Skeleton")]
            EnchantmentSummonSkeletonsNumAdditionalWarriorSkeletons1Updated, //Merciless Labyrinth

            [StringValue("40% chance to Summon an additional Skeleton Warrior with Summon Skeleton")]
            EnchantmentSummonSkeletonsNumAdditionalWarriorSkeletons2Updated, //Eternal Labyrinth

            [StringValue("Stone Golems deal 25% increased Damage")]
            EnchantmentSummonStoneGolemDamage1, //Merciless Labyrinth

            [StringValue("Stone Golems deal 40% increased Damage")]
            EnchantmentSummonStoneGolemDamage2, //Eternal Labyrinth

            [StringValue("Chaos Golems deal 25% increased Damage")]
            EnchantmentSumonChaosGolemDamage1, //Merciless Labyrinth

            [StringValue("Chaos Golems deal 40% increased Damage")]
            EnchantmentSumonChaosGolemDamage2, //Eternal Labyrinth

            [StringValue("10% increased Sunder Attack Speed")]
            EnchantmentSunderAttackSpeed1, //Merciless Labyrinth

            [StringValue("15% increased Sunder Attack Speed")]
            EnchantmentSunderAttackSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Sunder Damage")]
            EnchantmentSunderDamage1, //Merciless Labyrinth

            [StringValue("40% increased Sunder Damage")]
            EnchantmentSunderDamage2_, //Eternal Labyrinth

            [StringValue("8% increased Sunder Area of Effect")]
            EnchantmentSunderRadius1, //Merciless Labyrinth

            [StringValue("12% increased Sunder Area of Effect")]
            EnchantmentSunderRadius2,                                         //Eternal Labyrinth
            [StringValue("25% increased Sweep Damage")] EnchantmentSweepDamage1,   //Merciless Labyrinth
            [StringValue("40% increased Sweep Damage")] EnchantmentSweepDamage2__, //Eternal Labyrinth

            [StringValue("+20% Sweep Knockback Chance")]
            EnchantmentSweepKnockbackChance1, //Merciless Labyrinth

            [StringValue("+30% Sweep Knockback Chance")]
            EnchantmentSweepKnockbackChance2, //Eternal Labyrinth

            [StringValue("8% increased Sweep Area of Effect")]
            EnchantmentSweepRadius1_, //Merciless Labyrinth

            [StringValue("12% increased Sweep Area of Effect")]
            EnchantmentSweepRadius2, //Eternal Labyrinth

            [StringValue("8% increased Tectonic Slam Area of Effect")]
            EnchantmentTectonicSlamAreaOfEffect1, //Merciless Labyrinth

            [StringValue("12% increased Tectonic Slam Area of Effect")]
            EnchantmentTectonicSlamAreaOfEffect2, //Eternal Labyrinth

            [StringValue("25% chance for Tectonic Slam to not consume an Endurance Charge")]
            EnchantmentTectonicSlamChanceToNotConsumeCharge1, //Merciless Labyrinth

            [StringValue("40% chance for Tectonic Slam to not consume an Endurance Charge")]
            EnchantmentTectonicSlamChanceToNotConsumeCharge2, //Eternal Labyrinth

            [StringValue("25% increased Tectonic Slam Damage")]
            EnchantmentTectonicSlamDamage1, //Merciless Labyrinth

            [StringValue("40% increased Tectonic Slam Damage")]
            EnchantmentTectonicSlamDamage2, //Eternal Labyrinth

            [StringValue("25% increased Tempest Shield Damage")]
            EnchantmentTempestShieldDamage1__, //Merciless Labyrinth

            [StringValue("40% increased Tempest Shield Damage")]
            EnchantmentTempestShieldDamage2, //Eternal Labyrinth

            [StringValue("Tempest Shield chains an additional 2 times")]
            EnchantmentTempestShieldNumOfAdditionalProjectilesInChain1, //Merciless Labyrinth

            [StringValue("Tempest Shield chains an additional 3 times")]
            EnchantmentTempestShieldNumOfAdditionalProjectilesInChain2, //Eternal Labyrinth

            [StringValue("20% increased Temporal Chains Curse Effect")]
            EnchantmentTemporalChainsCurseEffect1, //Merciless Labyrinth

            [StringValue("30% increased Temporal Chains Curse Effect")]
            EnchantmentTemporalChainsCurseEffect2, //Eternal Labyrinth

            [StringValue("30% increased Temporal Chains Duration")]
            EnchantmentTemporalChainsDuration1, //Merciless Labyrinth

            [StringValue("45% increased Temporal Chains Duration")]
            EnchantmentTemporalChainsDuration2, //Eternal Labyrinth

            [StringValue("60% increased Tornado Shot Critical Strike Chance")]
            EnchantmentTornadoShotCriticalStrikeChance1, //Merciless Labyrinth

            [StringValue("90% increased Tornado Shot Critical Strike Chance")]
            EnchantmentTornadoShotCriticalStrikeChance2, //Eternal Labyrinth

            [StringValue("25% increased Tornado Shot Damage")]
            EnchantmentTornadoShotDamage1, //Merciless Labyrinth

            [StringValue("40% increased Tornado Shot Damage")]
            EnchantmentTornadoShotDamage2, //Eternal Labyrinth

            [StringValue("Tornado Shot fires an additional secondary Projectile")]
            EnchantmentTornadoShotNumOfSecondaryProjectiles1, //Merciless Labyrinth

            [StringValue("Tornado Shot fires 2 additional secondary Projectiles")]
            EnchantmentTornadoShotNumOfSecondaryProjectiles2, //Eternal Labyrinth

            [StringValue("20% increased Vengeance Cooldown Recovery Speed")]
            EnchantmentVengeanceCooldownSpeed1, //Merciless Labyrinth

            [StringValue("30% increased Vengeance Cooldown Recovery Speed")]
            EnchantmentVengeanceCooldownSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Vengeance Damage")]
            EnchantmentVengeanceDamage1, //Merciless Labyrinth

            [StringValue("40% increased Vengeance Damage")]
            EnchantmentVengeanceDamage2, //Eternal Labyrinth

            [StringValue("25% increased Vigilant Strike Damage")]
            EnchantmentVigilantStrikeDamage1, //Merciless Labyrinth

            [StringValue("40% increased Vigilant Strike Damage")]
            EnchantmentVigilantStrikeDamage2, //Eternal Labyrinth

            [StringValue("30% increased Vigilant Strike Fortify Duration")]
            EnchantmentVigilantStrikeFortifyDuration1, //Merciless Labyrinth

            [StringValue("45% increased Vigilant Strike Fortify Duration")]
            EnchantmentVigilantStrikeFortifyDuration2_, //Eternal Labyrinth

            [StringValue("60% increased Viper Strike Critical Strike Chance")]
            EnchantmentViperStrikeCriticalStrikeChance1, //Merciless Labyrinth

            [StringValue("90% increased Viper Strike Critical Strike Chance")]
            EnchantmentViperStrikeCriticalStrikeChance2, //Eternal Labyrinth

            [StringValue("25% increased Viper Strike Damage")]
            EnchantmentViperStrikeDamage1, //Merciless Labyrinth

            [StringValue("40% increased Viper Strike Damage")]
            EnchantmentViperStrikeDamage2, //Eternal Labyrinth

            [StringValue("20% increased Viper Strike Duration")]
            EnchantmentViperStrikePoisonDuration1_, //Merciless Labyrinth

            [StringValue("30% increased Viper Strike Duration")]
            EnchantmentViperStrikePoisonDuration2__, //Eternal Labyrinth

            [StringValue("14% reduced Vitality Mana Reservation")]
            EnchantmentVitalityManaReservation1, //Merciless Labyrinth

            [StringValue("20% reduced Vitality Mana Reservation")]
            EnchantmentVitalityManaReservation2, //Eternal Labyrinth

            [StringValue("8% increased Volatile Dead Cast Speed")]
            EnchantmentVolatileDeadCastSpeed1, //Merciless Labyrinth

            [StringValue("12% increased Volatile Dead Cast Speed")]
            EnchantmentVolatileDeadCastSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Volatile Dead Damage")]
            EnchantmentVolatileDeadDamage1, //Merciless Labyrinth

            [StringValue("40% increased Volatile Dead Damage")]
            EnchantmentVolatileDeadDamage2, //Eternal Labyrinth

            [StringValue("Volatile Dead destroys up to 1 additional Corpse")]
            EnchantmentVolatileDeadOrbs3, //Eternal Labyrinth

            [StringValue("25% increased Vortex Damage")]
            EnchantmentVortexDamage1, //Merciless Labyrinth

            [StringValue("40% increased Vortex Damage")]
            EnchantmentVortexDamage2, //Eternal Labyrinth

            [StringValue("20% increased Vortex Duration")]
            EnchantmentVortexDuration1, //Merciless Labyrinth

            [StringValue("30% increased Vortex Duration")]
            EnchantmentVortexDuration2, //Eternal Labyrinth

            [StringValue("8% increased Vortex Area of Effect")]
            EnchantmentVortexRadius1, //Merciless Labyrinth

            [StringValue("12% increased Vortex Area of Effect")]
            EnchantmentVortexRadius2, //Eternal Labyrinth

            [StringValue("20% increased Vulnerability Curse Effect")]
            EnchantmentVulnerabilityCurseEffect1, //Merciless Labyrinth

            [StringValue("30% increased Vulnerability Curse Effect")]
            EnchantmentVulnerabilityCurseEffect2, //Eternal Labyrinth

            [StringValue("30% increased Vulnerability Duration")]
            EnchantmentVulnerabilityDuration1, //Merciless Labyrinth

            [StringValue("45% increased Vulnerability Duration")]
            EnchantmentVulnerabilityDuration2, //Eternal Labyrinth

            [StringValue("20% increased Warlord's Mark Curse Effect")]
            EnchantmentWarlordsMarkCurseEffect1, //Merciless Labyrinth

            [StringValue("30% increased Warlord's Mark Curse Effect")]
            EnchantmentWarlordsMarkCurseEffect2, //Eternal Labyrinth

            [StringValue("30% increased Warlord's Mark Duration")]
            EnchantmentWarlordsMarkDuration1_, //Merciless Labyrinth

            [StringValue("45% increased Warlord's Mark Duration")]
            EnchantmentWarlordsMarkDuration2, //Eternal Labyrinth

            [StringValue("10% increased Whirling Blades Attack Speed")]
            EnchantmentWhirlingBladesAttackSpeed1, //Merciless Labyrinth

            [StringValue("15% increased Whirling Blades Attack Speed")]
            EnchantmentWhirlingBladesAttackSpeed2, //Eternal Labyrinth

            [StringValue("25% increased Whirling Blades Damage")]
            EnchantmentWhirlingBladesDamage1, //Merciless Labyrinth

            [StringValue("40% increased Whirling Blades Damage")]
            EnchantmentWhirlingBladesDamage2, //Eternal Labyrinth

            [StringValue("25% increased Wild Strike Damage")]
            EnchantmentWildStrikeDamage1, //Merciless Labyrinth

            [StringValue("40% increased Wild Strike Damage")]
            EnchantmentWildStrikeDamage2, //Eternal Labyrinth

            [StringValue("Wild Strike Chains an additional 4 times")]
            EnchantmentWildStrikeNumOfAdditionalProjectilesInChain1, //Merciless Labyrinth

            [StringValue("Wild Strike Chains an additional 6 times")]
            EnchantmentWildStrikeNumOfAdditionalProjectilesInChain2, //Eternal Labyrinth

            [StringValue("24% increased Wild Strike Area of Effect")]
            EnchantmentWildStrikeRadius1, //Merciless Labyrinth

            [StringValue("36% increased Wild Strike Area of Effect")]
            EnchantmentWildStrikeRadius2, //Eternal Labyrinth

            [StringValue("24% increased Wither Duration")]
            EnchantmentWitherDuration1, //Merciless Labyrinth

            [StringValue("36% increased Wither Duration")]
            EnchantmentWitherDuration2, //Eternal Labyrinth

            [StringValue("8% increased Wither Area of Effect")]
            EnchantmentWitherRadius1, //Merciless Labyrinth

            [StringValue("12% increased Wither Area of Effect")]
            EnchantmentWitherRadius2, //Eternal Labyrinth

            [StringValue("10% reduced Wrath Mana Reservation")]
            EnchantmentWrathManaReservation1, //Merciless Labyrinth

            [StringValue("15% reduced Wrath Mana Reservation")]
            EnchantmentWrathManaReservation2, //Eternal Labyrinth

            [StringValue("Zombies have 10% increased Attack Speed")]
            EnchantmentZombieAttackSpeed1, //Merciless Labyrinth

            [StringValue("Zombies have 15% increased Attack Speed")]
            EnchantmentZombieAttackSpeed2, //Eternal Labyrinth

            [StringValue("+24% to Raised Zombie Elemental Resistances")]
            EnchantmentZombieElementalResistances1, //Merciless Labyrinth

            [StringValue("+36% to Raised Zombie Elemental Resistances")]
            EnchantmentZombieElementalResistances2 //Eternal Labyrinth
        }
    }
}