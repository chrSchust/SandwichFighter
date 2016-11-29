using System.Collections.Generic;


public interface Ingredient

{
    string getName();
    int getDamageBonus(int enemyType);
    int getSpeedBonus(int enemyType);
}