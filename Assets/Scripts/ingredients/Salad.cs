
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
                return +10;
            case Enemy.VEGAN:
                return -10;
            default: return 0;
        }
    }

    public int getSpeedBonus(int enemyType)
    {
        switch (enemyType)
        {
            case Enemy.FAT:
                return +3;
            case Enemy.VEGAN:
                return -3;
            default: return 0;
        }
    }
}
