using MegaCrit.Sts2.Core.Commands;

namespace Ace.AceCode.Extensions;

public static class AudioHelper
{
    private static readonly Random rng = new Random();

    private static readonly string[] attackSfx =
    {
        "res://Ace/sounds/attack (1).wav",
        "res://Ace/sounds/attack (2).wav",
        "res://Ace/sounds/attack (3).wav",
        "res://Ace/sounds/attack (4).wav",
        "res://Ace/sounds/attack (5).wav",
        "res://Ace/sounds/attack (6).wav",
    };
    
    private static readonly string[] attackMediumSfx =
    {
        "res://Ace/sounds/attack_medium (1).wav",
        "res://Ace/sounds/attack_medium (2).wav",
        "res://Ace/sounds/attack_medium (3).wav",
        "res://Ace/sounds/attack_medium (4).wav",
        "res://Ace/sounds/attack_medium (5).wav",
        "res://Ace/sounds/attack_medium (6).wav",
        "res://Ace/sounds/attack_medium (7).wav",
        "res://Ace/sounds/attack_medium (8).wav",
        "res://Ace/sounds/attack_medium (9).wav",
        "res://Ace/sounds/attack_medium (10).wav",
        "res://Ace/sounds/attack_medium (11).wav",
        "res://Ace/sounds/attack_medium (12).wav",
        "res://Ace/sounds/attack_medium (13).wav",
        
    };
    
    private static readonly string[] damagedSfx =
    {
        "res://Ace/sounds/hit_small (1).wav",
        "res://Ace/sounds/hit_small (2).wav",
        "res://Ace/sounds/hit_small (3).wav",
        "res://Ace/sounds/hit_small (4).wav"
    };
    
    private static readonly string[] highDamagedSfx =
    {
        "res://Ace/sounds/hit_high (1).wav",
        "res://Ace/sounds/hit_high (2).wav",
        "res://Ace/sounds/hit_high (3).wav",
        "res://Ace/sounds/hit_high (4).wav",
    };
    
    private static readonly string[] criticalDamagedSfx =
    {
        "res://Ace/sounds/hit_critical (1).wav",
        "res://Ace/sounds/hit_critical (2).wav",
        "res://Ace/sounds/hit_critical (3).wav",
        "res://Ace/sounds/hit_critical (4).wav",
    };
    
    private static readonly string[] attackCriticalSfx =
    {
        "res://Ace/sounds/attack_critical (1).wav",
        "res://Ace/sounds/attack_critical (2).wav",
        "res://Ace/sounds/attack_critical (3).wav",
    };
    
    private static readonly string[] victorySfx =
    {
        "res://Ace/sounds/victory_1.wav",
        "res://Ace/sounds/victory_2.wav",
        "res://Ace/sounds/victory_3.wav",
        "res://Ace/sounds/victory_4.wav",
        "res://Ace/sounds/victory_5.wav",
        "res://Ace/sounds/victory_6.wav",
        "res://Ace/sounds/victory_7.wav",
    };

    private static readonly string[] gameoverSfx =
    {
        "res://Ace/sounds/gameover (1).wav",
        "res://Ace/sounds/gameover (2).wav",
        "res://Ace/sounds/gameover_1.wav",
        "res://Ace/sounds/gameover_2.wav",
    };
    
    private static readonly string[] fireSfx =
    {
        "res://Ace/sounds/fire_1.wav",
        "res://Ace/sounds/fire_2.wav",
    };
    
    private static readonly string[] iceSfx =
    {
        "res://Ace/sounds/ice_1.wav",
        "res://Ace/sounds/ice_2.wav",
        "res://Ace/sounds/ice_3.wav",
    };
    
    private static readonly string[] thunderSfx =
    {
        "res://Ace/sounds/thunder_1.wav",
        "res://Ace/sounds/thunder_2.wav",
        "res://Ace/sounds/thunder_3.wav",
    };

    private static readonly string[] attackHighSfx =
    {
        "res://Ace/sounds/attack_hard (1).wav",
        "res://Ace/sounds/attack_hard (2).wav",
        "res://Ace/sounds/attack_hard (3).wav",
        "res://Ace/sounds/attack_hard (4).wav",
        "res://Ace/sounds/attack_hard (5).wav",
    };
    
    private static readonly string[] lastHitSfx =
    {
        "res://Ace/sounds/last_hit (1).wav",
        "res://Ace/sounds/last_hit (2).wav",
        "res://Ace/sounds/last_hit (3).wav",
        "res://Ace/sounds/last_hit (4).wav",
        "res://Ace/sounds/last_hit (5).wav",
        "res://Ace/sounds/last_hit (6).wav",
        "res://Ace/sounds/last_hit (7).wav",
        "res://Ace/sounds/last_hit (8).wav",
        "res://Ace/sounds/last_hit (9).wav",
    };
    
    private static readonly string[] limitBreakSfx =
    {
        "res://Ace/sounds/ultimate (1).wav",
        "res://Ace/sounds/ultimate (2).wav",
        "res://Ace/sounds/ultimate (3).wav",
        "res://Ace/sounds/ultimate (4).wav",
        "res://Ace/sounds/ultimate (5).wav",
    };
    
    public static void PlayRandomAttack()
    {
        PlayRandom(attackSfx);
    }
    
    public static void PlayRandomAttackCritical()
    {
        PlayRandom(attackCriticalSfx);
    }

    public static void PlayRandomAttackMedium()
    {
        PlayRandom(attackMediumSfx);
    }
    
    public static void PlayRandomDamaged()
    {
        PlayRandom(damagedSfx);
    }

    public static void PlayRandomDamagedHigh()
    {
        PlayRandom(highDamagedSfx);
    }

    public static void PlayRandomGameover()
    {
        PlayRandom(gameoverSfx);
    }

    public static void PlayRandomDamagedCritical()
    {
        PlayRandom(criticalDamagedSfx);
    }
    
    public static void PlayRandomVictory()
    {
        PlayRandom(victorySfx);
    }

    public static void PlayRandomAttackHard()
    {
        PlayRandom(attackHighSfx);
    }

    public static void PlayRandomLastHit()
    {
        PlayRandom(lastHitSfx);
    }

    public static void PlayRandomLimitBreak()
    {
        PlayRandom(limitBreakSfx);
    }

    public static void PlayRandomFire()
    {
        PlayRandom(fireSfx);
    }
    
    public static void PlayRandomIce()
    {
        PlayRandom(iceSfx);
    }

    public static void PlayRandom(string[] pool)
    {
        int index = rng.Next(pool.Length);
        SfxCmd.Play(pool[index]);
    }
}