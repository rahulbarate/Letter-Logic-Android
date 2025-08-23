using System.Collections.Generic;

public class TempDatabaseManger
{
    static List<Word> wordsOfLength3 = new List<Word>(new[]{
            new Word{Text= "cat", Hint="A small furry animal that purrs when happy", IsUsed= 0, TextLength= 3 },
            new Word{Text= "bat", Hint="A flying mammal that comes out at night", IsUsed= 0, TextLength= 3 },
            new Word{Text= "sun", Hint="A bright ball of fire in the sky", IsUsed= 0, TextLength= 3 },
            new Word{Text= "dog", Hint="A loyal friend that loves to wag its tail", IsUsed= 0, TextLength= 3 },
            new Word{Text= "bug", Hint="A tiny creature with six legs that crawls", IsUsed= 0, TextLength= 3 },
            new Word{Text= "bee", Hint="A buzzing insect that makes honey", IsUsed= 0, TextLength= 3 },
            new Word{Text= "hat", Hint="Something you wear on your head", IsUsed= 0, TextLength= 3 },
            new Word{Text= "bed", Hint="A comfy place to sleep at night", IsUsed= 0, TextLength= 3 },
            new Word{Text= "car", Hint="A vehicle that takes you places", IsUsed= 0, TextLength= 3 },
            new Word{Text= "jar", Hint="A container with a lid for holding things", IsUsed= 0, TextLength= 3 },
            new Word{ Text= "pen", Hint= "A tool to write or draw with ink", IsUsed= 0, TextLength= 3 },
            new Word{ Text = "map", Hint= "A paper that shows you where to go", IsUsed= 0, TextLength= 3 },
            new Word{ Text = "bus", Hint= "A big vehicle that takes many people", IsUsed= 0, TextLength= 3 },
            new Word{ Text = "net", Hint= "Something used to catch fish or butterflies", IsUsed= 0, TextLength= 3 },
            new Word{ Text = "box", Hint= "A container for storing things", IsUsed= 0, TextLength= 3 },
            new Word{ Text = "fox", Hint= "A clever animal with a bushy tail", IsUsed= 0, TextLength= 3 },
            new Word{ Text = "cap", Hint= "A type of hat that shields your face from the sun", IsUsed= 0, TextLength= 3 },
            new Word{ Text = "egg", Hint= "Something you crack open for breakfast", IsUsed= 0, TextLength= 3 },
            new Word{ Text = "rug", Hint= "A soft cover for the floor", IsUsed= 0, TextLength= 3 },
            new Word{ Text = "mug", Hint= "A cup you use for hot drinks", IsUsed= 0, TextLength= 3 }
        });
    static List<Word> wordsOfLength4 = new List<Word>(new[]{
            new Word{ Text = "frog", Hint= "A green animal that hops and croaks", IsUsed= 0, TextLength= 4 },
            new Word{ Text = "tree", Hint= "A tall plant with leaves and branches", IsUsed= 0, TextLength= 4 },
            new Word{ Text = "fish", Hint= "A creature that swims in water", IsUsed= 0, TextLength= 4 },
            new Word{ Text = "star", Hint= "A bright object that twinkles in the night sky", IsUsed= 0, TextLength= 4 },
            new Word{ Text = "boat", Hint= "A vehicle that floats on water", IsUsed= 0, TextLength= 4 },
            new Word{ Text = "lion", Hint= "The king of the jungle with a loud roar", IsUsed= 0, TextLength= 4 },
            new Word{ Text = "cake", Hint= "A sweet treat often eaten at parties", IsUsed= 0, TextLength= 4 },
            new Word{ Text = "book", Hint= "A collection of pages you read", IsUsed= 0, TextLength= 4 },
            new Word{ Text = "ball", Hint= "A round object used in many sports", IsUsed= 0, TextLength= 4 },
            new Word{ Text = "milk", Hint= "A white drink that comes from cows", IsUsed= 0, TextLength= 4 },
            new Word{ Text = "corn", Hint= "A yellow vegetable that grows on a cob", IsUsed= 0, TextLength= 4 },
            new Word{ Text = "bird", Hint= "An animal that flies in the sky", IsUsed= 0, TextLength= 4 },
            new Word{ Text = "wind", Hint= "Air that moves and makes things sway", IsUsed= 0, TextLength= 4 },
            new Word{ Text = "rock", Hint= "A hard object found on the ground", IsUsed= 0, TextLength= 4 },
            new Word{ Text = "snow", Hint= "White and cold, it falls in winter", IsUsed= 0, TextLength= 4 },
            new Word{ Text = "fire", Hint= "Hot and bright, it gives warmth and light", IsUsed= 0, TextLength= 4 },
            new Word{ Text = "gift", Hint= "Something wrapped to give someone on special days", IsUsed= 0, TextLength= 4 },
            new Word{ Text = "moon", Hint= "A round light in the night sky", IsUsed= 0, TextLength= 4 },
            new Word{ Text = "nest", Hint= "A bird's home in a tree", IsUsed= 0, TextLength= 4 },
            new Word{ Text = "sand", Hint= "Tiny grains found on a beach", IsUsed= 0, TextLength= 4 }});
    static List<Word> wordsOfLength5 = new List<Word>(new[]{
            new Word{ Text = "apple", Hint= "A red or green fruit that's crunchy and sweet", IsUsed= 0, TextLength= 5 },
            new Word{ Text = "bread", Hint= "A staple food made from flour and baked", IsUsed= 0, TextLength= 5 },
            new Word{ Text = "chair", Hint= "Something you sit on", IsUsed= 0, TextLength= 5 },
            new Word{ Text = "grass", Hint= "Green and grows in your yard", IsUsed= 0, TextLength= 5 },
            new Word{ Text = "house", Hint= "A place where people live", IsUsed= 0, TextLength= 5 },
            new Word{ Text = "horse", Hint= "An animal you can ride", IsUsed= 0, TextLength= 5 },
            new Word{ Text = "plant", Hint= "Something green that grows in soil", IsUsed= 0, TextLength= 5 },
            new Word{ Text = "table", Hint= "A piece of furniture where you eat or work", IsUsed= 0, TextLength= 5 },
            new Word{ Text = "ocean", Hint= "A big body of salty water", IsUsed= 0, TextLength= 5 },
            new Word{ Text = "robot", Hint= "A machine that can do tasks for you", IsUsed= 0, TextLength= 5 },
            new Word{ Text = "cloud", Hint= "White and fluffy in the sky", IsUsed= 0, TextLength= 5 },
            new Word{ Text = "zebra", Hint= "A striped animal that looks like a horse", IsUsed= 0, TextLength= 5 },
            new Word{ Text = "piano", Hint= "A musical instrument with keys", IsUsed= 0, TextLength= 5 },
            new Word{ Text = "sheep", Hint= "A fluffy animal that gives wool", IsUsed= 0, TextLength= 5 },
            new Word{ Text = "pizza", Hint= "A delicious dish with cheese and toppings", IsUsed= 0, TextLength= 5 },
            new Word{ Text = "train", Hint= "A vehicle that runs on tracks", IsUsed= 0, TextLength= 5 },
            new Word{ Text = "tiger", Hint= "A big striped cat in the wild", IsUsed= 0, TextLength= 5 },
            new Word{ Text = "candy", Hint= "A sweet treat kids love", IsUsed= 0, TextLength= 5 },
            new Word{ Text = "grape", Hint= "A small round fruit that grows in bunches", IsUsed= 0, TextLength= 5 },
            new Word{ Text = "light", Hint= "Something that helps you see in the dark", IsUsed= 0, TextLength= 5 }});

    public static List<Word> GetWords(int wordLength, int numberOfWords)
    {
        List<Word> wordsOfGivenLength = new List<Word>();
        List<Word> words = new List<Word>();
        if (wordLength == 3)
            words = wordsOfLength3;
        else if (wordLength == 4)
            words = wordsOfLength4;
        else if (wordLength == 5)
            words = wordsOfLength5;

        int i = 0;

        foreach (Word word in words)
        {
            if (i == numberOfWords)
            {
                break;
            }
            wordsOfGivenLength.Add(word);
            i++;
        }
        return wordsOfGivenLength;

    }
}