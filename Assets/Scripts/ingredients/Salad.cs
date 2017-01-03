
using System;

public class Salad : Ingredient
{
    public string getName()
    {
        return "Salat";
    }

	public string getType() 
	{ 
		return IngredientType.VEGETABLE; 
	}

    public int getDamageBonus(int enemyType)
    {
        switch (enemyType)
        {
            case Enemy.FAT:
                // return +10;
				return +30;
            case Enemy.VEGAN:
                // return -10;
				return 0;
            default: return 0;
        }
    }

    public int getSpeedBonus(int enemyType)
    {
        switch (enemyType)
        {
            case Enemy.FAT:
                // return +3;
				return +2;
            case Enemy.VEGAN:
                // return -3;
				return -1;
            default: return 0;
        }
    }
}
