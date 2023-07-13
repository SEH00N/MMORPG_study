using System.Reflection;

namespace CSharpBasicSyntax
{
    internal class Reflection
    {
        class Important : Attribute
        {
            string message;

            public Important(string message) { this.message = message; }
        }

        class Monster
        {
            [Important("Very Important")]
            public int hp;
            protected int attack;
            private float speed;

            void Attack() { }
        }

        //static void Main(string[] args)
        //{
        //    Monster monster = new Monster();
        //    Type type = monster.GetType();

        //    FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

        //    foreach(FieldInfo field in fields)
        //    {
        //        string access = "protected";
        //        if (field.IsPublic)
        //            access = "public";
        //        else if (field.IsPrivate)
        //            access = "private";

        //        Console.WriteLine($"{access} {field.FieldType.Name} {field.Name}");
        //    }
        //}
    }
}
