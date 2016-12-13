using System.Collections.Generic;


public interface Ingredient

{
	string getName();
	string getType ();
    int getDamageBonus(int enemyType);
    int getSpeedBonus(int enemyType);
}