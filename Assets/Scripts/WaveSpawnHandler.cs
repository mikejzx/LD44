using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Yes i know i shouldn'tve hardcoded this ffs 
*/

public static class WaveSpawnHandler {

    public static void GetEnemyCountsTyped (int wave, int total, out int ebasic,
        out int etough, out int etougher, out int einsane) {

        // Some of these are hard-coded...
        if (wave < 7) {
            switch (wave) {
                case (1): { ebasic = total; etough = 0; etougher = 0; einsane = 0; } return;
                case (2): { ebasic = total - 5; etough = 5; etougher = 0; einsane = 0; } return;
                case (3): { ebasic = total - 10; etough = 5; etougher = 5; einsane = 0; } return;
                case (4): { ebasic = total - 20; etough = 9; etougher = 10; einsane = 1; } return;
                case (5): { ebasic = total - 20; etough = 9; etougher = 10; einsane = 3; } return;
                case (6): { ebasic = total - 20; etough = 15; etougher = 15; einsane = 10; } return;
            }
        }
        int eight = total / 8;
        ebasic = eight * 2;
        etough = eight * 3;
        etougher = eight * 2;
        einsane = eight;
    }
}
