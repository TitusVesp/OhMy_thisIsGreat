using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace OhMy_thisIsGreat
{
    class Program
    {
        // Foo1 and Foo2 are equivalent:
        static int Foo1(int x) { return x * 2; }
        static int Foo2(int x) => x * 2;

        public class Wine
        {
            public decimal Price;
            public int Year;
            public Wine(decimal price) { Price = price; }
            public Wine(decimal price, int year) : this(price) { Year = year; }
        }
        public class Class1
        {
            Class1() { }        // Private constructor

            public static Class1 Create()
            {
                // Perform custom logic here to create & configure an instance of Class1
                /* ... */
                return new Class1();
            }
        }

        class Rectangle
        {
            public readonly float Width, Height;

            public Rectangle(float width, float height)
            {
                Width = width;
                Height = height;
            }

            public void Deconstruct(out float width, out float height)
            {
                width = Width;
                height = Height;
            }
        }

        public class Bunny
        {
            public string Name;
            public bool LikesCarrots;
            public bool LikesHumans;

            public Bunny() { }
            public Bunny(string n) { Name = n; }
        }

        // The this reference refers to the instance itself:
        public class Panda
        {
            public Panda Mate;

            public void Marry(Panda partner)
            {
                Mate = partner;
                partner.Mate = this;
            }
        }

        // Properties look like fields from the outside but internally, they contain logic, like methods:
        public class Stock1
        {
            decimal currentPrice;           // The private "backing" field

            public decimal CurrentPrice     // The public property
            {
                get { return currentPrice; }
                set { currentPrice = value; }
            }
        }
        public class Stock2
        {
            decimal currentPrice;           // The private "backing" field
            public decimal CurrentPrice     // The public property
            {
                get { return currentPrice; }
                set { currentPrice = value; }
            }

            decimal sharesOwned;           // The private "backing" field
            public decimal SharesOwned     // The public property
            {
                get { return sharesOwned; }
                set { sharesOwned = value; }
            }

            public decimal Worth
            {
                get { return currentPrice * sharesOwned; }
            }
        }
        public class Stock3
        {
            decimal currentPrice;           // The private "backing" field
            public decimal CurrentPrice     // The public property
            {
                get { return currentPrice; }
                set { currentPrice = value; }
            }

            decimal sharesOwned;           // The private "backing" field
            public decimal SharesOwned     // The public property
            {
                get { return sharesOwned; }
                set { sharesOwned = value; }
            }

            public decimal Worth => currentPrice * sharesOwned;    // Expression-bodied property

            // In C# 7, we can take this further, and write both the get and set accessors in expression-bodied syntax:
            public decimal Worth2
            {
                get => currentPrice * sharesOwned;
                set => sharesOwned = value / currentPrice;
            }

        }
        public class Stock4
        {
            public decimal CurrentPrice { get; set; } = 99;  // Automatic property
            public decimal SharesOwned { get; set; } = 123;   // Automatic property

            public decimal Worth
            {
                get => CurrentPrice * SharesOwned;
            }
        }
        public class Foo
        {
            private decimal x;
            public decimal X
            {
                get { return x; }
                private set { x = Math.Round(value, 2); }
            }

            public int Auto { get; private set; }   // Automatic property
        }

        // You can implement custom indexers with the this keyword:
        class Sentence
        {
            string[] words = "The quick brown fox".Split();

            public string this[int wordNum]      // indexer
            {
                get { return words[wordNum]; }
                set { words[wordNum] = value; }
            }
        }

        // A static constructor executes once per type, rather than once per instance:
        class Test
        {
            static Test()
            {
                Console.WriteLine("Type Initialized");
            }
        }
        class FooSt
        {
            public static int X = Y;    // 0
            public static int Y = 3;    // 3
        }
        class FooSt2
        {
            public static Foo Instance = new Foo();
            public static int X = 3;

            FooSt2() { Console.WriteLine(X); }   // 0
        }

        // Partial types allow a type definition to be split—typically across multiple files:
        partial class PaymentForm2 { public int X; }
        partial class PaymentForm2 { public int Y; }

        // A partial type may contain partial methods. These let an auto-generated partial type
        // provide customizable hooks for manual authoring.

        partial class PaymentForm    // In auto-generated file
        {
            public PaymentForm(decimal amount)
            {
                ValidatePayment(amount);
                // ...
            }

            partial void ValidatePayment(decimal amount);
        }

        partial class PaymentForm    // In hand-authored file
        {
            partial void ValidatePayment(decimal amount)
            {
                if (amount < 100)
                    throw new ArgumentOutOfRangeException("amount", "Amount too low!");
            }
        }

        // A function marked as virtual can be overridden by subclasses wanting to provide a specialized implementation:
        public class Asset
        {
            public string Name;
            public virtual decimal Liability => 0;      // Virtual
        }

        public class House : Asset
        {
            public decimal Mortgage;
            public override decimal Liability => Mortgage;   // Overridden
        }

        public class Stock : Asset
        {
            public long SharesOwned;
            // We won't override Liability here, because the default implementation will do.
        }
        // A variable of type x can refer to an object that subclasses x.
        public static void Display(Asset asset)
        {
            Console.WriteLine(asset.Name);
        }

        // A class declared as abstract can never be instantiated. Instead, only its concrete subclasses
        // can be instantiated. Abstract classes are able to define abstract members.
        public abstract class AssetA     // Note abstract keyword
        {
            public abstract decimal NetValue { get; }   // Note empty implementation
        }

        public class StockA : AssetA
        {
            public long SharesOwned;
            public decimal CurrentPrice;

            // Override like a virtual method.
            public override decimal NetValue => CurrentPrice * SharesOwned;
        }

        public class A { public int Counter = 1; }
        public class B : A { public int Counter = 2; }

        // Occasionally, you want to hide a member deliberately, in which case you can apply the new  
        // modifier to the member in the subclass, to avoid the compiler warning. The behavior is the same:

        public class X { public int Counter = 1; }
        public class Y : X { public new int Counter = 2; }

        public class BaseClass
        {
            public virtual void Foo() { Console.WriteLine("BaseClass.Foo"); }
        }

        public class Overrider : BaseClass
        {
            public override void Foo() { Console.WriteLine("Overrider.Foo"); }
        }

        public class Hider : BaseClass
        {
            public new void Foo() { Console.WriteLine("Hider.Foo"); }
        }

        // An overridden function member may seal its implementation with the sealed keyword to prevent it
        // from being overridden by further subclasses:

        // You can also seal the class itself, implicitly sealing all the virtual functions:

        // public sealed class Stock : Asset { /* ... */ }

        // A subclass must declare its own constructors. In doing so, it can call any of the
        // base class’s constructors with the base keyword:

        //Constructors & Inheritance
        public class Baseclass
        {
            public int X;
            public Baseclass() { }
            public Baseclass(int x) { this.X = x; }
        }

        public class Subclass : Baseclass
        {
            public Subclass(int x) : base(x) { }
        }

        // If a constructor in a subclass omits the base keyword, the base type’s parameterless
        // constructor is implicitly called:

        // Implicit Calling of the Parameterless Base Class Constructor
        public class BaseClass2
        {
            protected int X;
            public BaseClass2() { X = 1; }
        }

        public class Subclass2 : BaseClass2
        {
            public Subclass2() { Console.WriteLine(X); }  // 1
        }

        // When calling an overload method, the method with the most specific 
        // parameter type match has precedence, based on the *compile-time* variable type:


        //Overloading and Resolution
        static void FooI(Asset a) { Console.WriteLine("Foo Asset"); }
        static void FooI(House h) { Console.WriteLine("Foo House"); }

        // object (System.Object) is the ultimate base class for all types. Any type can be
        // implicitly converted to object; we can leverage this to write a general-purpose Stack:
        public class Stack
        {
            int position;
            object[] data = new object[10];
            public void Push(object obj) { data[position++] = obj; }
            public object Pop() { return data[--position]; }
        }
        public class Point { public int X, Y; }
        public class PandaStr
        {
            public string Name;
            public override string ToString() { return Name; }
        }

        // The IEnumerator interface is part of the .NET Framework, defined in System.Collections.
        // We can define our own version of this as follows:

        public interface IEnumerator
        {
            bool MoveNext();
            object Current { get; }
            void Reset();
        }

        // Here's a class that implements this interface:

        class Countdown : IEnumerator
        {
            int count = 11;
            public bool MoveNext() => count-- > 0;
            public object Current => count;
            public void Reset() { throw new NotSupportedException(); }
        }

        // We can extend interfaces - just like extending classes:

        public interface IUndoable { void Undo(); }
        public interface IRedoable : IUndoable { void Redo(); }

        // Implementing multiple interfaces can sometimes result in a collision between member signatures.
        // You can resolve such collisions by explicitly implementing an interface member:

        interface I1 { void Foo(); }
        interface I2 { int Foo(); }

        public class Widget : I1, I2
        {
            public void Foo()
            {
                Console.WriteLine("Widget's implementation of I1.Foo");
            }

            int I2.Foo()
            {
                Console.WriteLine("Widget's implementation of I2.Foo");
                return 42;
            }
        }

        public class TextBox : IUndoable
        {
            //void IUndoable.Undo()         => Undo();    // Calls method below
            public virtual void Undo() => Console.WriteLine("TextBox.Undo");
        }

        public class RichTextBox : TextBox
        {
            public override void Undo() => Console.WriteLine("RichTextBox.Undo");
        }

        // Casting a struct to an interface causes boxing. Calling an implicitly implemented
        // member on a struct does not cause boxing:

        interface I { void Foo(); }
        struct S : I { public void Foo() { } }

        // An enum is a special value type that lets you specify a group of named numeric constants:

        public enum BorderSide { Left, Right, Top, Bottom }

        // An invalid BorderSide would break the following method:

        void Draw(BorderSide side)
        {
            if (side == BorderSide.Left) { /*...*/ }
            else if (side == BorderSide.Right) { /*...*/ }
            else if (side == BorderSide.Top) { /*...*/ }
            else { /*...*/ }  // Assume BorderSide.Bottom
        }

        // You may specify an alternative integral type:
        public enum BorderSideByte : byte { Left, Right, Top, Bottom }

        // You may also specify an explicit underlying value for each enum member:
        public enum BorderSideExplicit : byte { Left = 1, Right = 2, Top = 10, Bottom = 11 }

        public enum BorderSidePartiallyExplicit : byte { Left = 1, Right, Top = 10, Bottom }

        public enum HorizontalAlignment
        {
            Left = BorderSide.Left,
            Right = BorderSide.Right,
            Center
        }


        // You can combine enum members. To prevent ambiguities, members of a combinable enum require
        // explicitly assigned values, typically in powers of two:

        [Flags]
        public enum BorderSides { None = 0, Left = 1, Right = 2, Top = 4, Bottom = 8 }

        static bool IsFlagDefined(Enum e)
        {
            decimal d;
            return !decimal.TryParse(e.ToString(), out d);
        }

        // For convenience, you can include combination members within an enum declaration itself:

        [Flags]
        public enum BorderSides2
        {
            None = 0,
            Left = 1, Right = 2, Top = 4, Bottom = 8,
            LeftRight = Left | Right,
            TopBottom = Top | Bottom,
            All = LeftRight | TopBottom
        }


        // A nested type is declared within the scope of another type. For example:

        public class TopLevel
        {
            public class Nested { }               // Nested class
            public enum Color { Red, Blue, Tan }  // Nested enum
        }

        public class TopLevel2
        {
            static int x;
            public class Nested
            {
                public static void Foo() { Console.WriteLine(TopLevel2.x); }
            }
        }
        public class TopLevel3
        {
            protected class Nested { }
        }

        public class SubTopLevel : TopLevel3
        {
            static void Foo() { new TopLevel3.Nested(); }
        }

        static void Main(string[] args)
        {
            goto mark;
            #region Expression-boiled
            {
                //fields, readonly, overloading
                // The following overloads are prohibited:

                //void Foo(int x);
                //float Foo(int x);           // Compile-time error

                //void Goo(int[] x);
                //void Goo(params int[] x);  // Compile-time error

                //void Hoo(int x);
                //void Hoo(ref int x);      // OK so far
                //void Hoo(out int x);      // Compile-time error

                int x = Foo1(10);
                int y = Foo2(10);
            }
            #endregion
            Console.ReadLine();

            #region local method
            {
                Console.WriteLine(Cube(4));

                int Cube(int value) => value * value * value;

                Console.WriteLine(Cube(3));
            }
            #endregion
            Console.ReadLine();

            #region Constructors
            {
                //Overloading!
                Class1 c1 = Class1.Create();    // OK
                //Class1 c2 = new Class1();		// Error: Will not compile
            }
            #endregion
            Console.ReadLine();

            #region Deconstructors
            {
                // To call the deconstructor, we use the following special syntax:
                var rect = new Rectangle(3, 4);
                (float width, float height) = rect;          // Deconstruction
                Console.WriteLine(width + " " + height);    // 3 4

                // We can also use implicit typing:  
                var (x, y) = rect;          // Deconstruction
                Console.WriteLine(x + " " + y);

                // If the variables already exist, we can do a *deconstructing assignment*:
                (y, x) = rect;
                Console.WriteLine(x + " " + y);
            }
            #endregion
            Console.ReadLine();

            #region Object Initializers
            {
                // Object initialization syntax. Note that we can still specify constructor arguments:

                Bunny b1 = new Bunny { Name = "Bo", LikesCarrots = true, LikesHumans = false };
                Bunny b2 = new Bunny("Bo") { LikesCarrots = true, LikesHumans = false };

                // Instead of using object initializers, we could make Bunny’s constructor accept optional parameters.
                // This has both pros and cons unsafe to use optional pareameters, must be repackaging!
            }
            #endregion
            Console.ReadLine();

            #region the this reference
            {
                new Panda().Marry(new Panda());
            }
            #endregion
            Console.ReadLine();

            #region Properties
            {
                var stock = new Stock1();
                stock.CurrentPrice = 123.45M;
                Console.WriteLine(stock.CurrentPrice);

                var stock2 = new Stock1 { CurrentPrice = 83.12M };
                Console.WriteLine(stock2.CurrentPrice);

                //Stock2 and Stock3!! - AutomaticProperties is Stock4! Foo)
            }
            #endregion
            Console.ReadLine();

            #region Indexers
            {
                Sentence s = new Sentence();
                Console.WriteLine(s[3]);       // fox
                s[3] = "kangaroo";
                Console.WriteLine(s[3]);       // kangaroo

                //Constants scoped to class and Conctants scoped to methods 
                double x = 2 * System.Math.E;
            }
            #endregion
            Console.ReadLine();

            #region Static Constructors
            {
                // Type is initialized only once
                new Test();
                new Test();
                new Test();

                Console.WriteLine(FooSt.X + " " + FooSt.Y);

                Console.WriteLine(FooSt2.X);
            }
            #endregion
            Console.ReadLine();

            #region Partial types + nameof operator
            {
                Console.WriteLine(new PaymentForm2 { X = 3, Y = 4 });

                var paymentForm = new PaymentForm(150);

                int count = 123;
                Console.WriteLine(nameof(count) + ("  count"));

                Console.WriteLine(nameof(StringBuilder.Length) + ("  Length property on StringBuilder"));

                Console.WriteLine((nameof(StringBuilder) + "." + nameof(StringBuilder.Length)) + ("  StringBuilder.Length"));
            }
            #endregion
            Console.ReadLine();

            #region Inheritance
            {
                Stock msft = new Stock { Name = "MSFT", SharesOwned = 1000 };

                Console.WriteLine(msft.Name);         // MSFT
                Console.WriteLine(msft.SharesOwned);  // 1000

                House mansion = new House { Name = "Mansion", Mortgage = 250000 };

                Console.WriteLine(mansion.Name);      // Mansion
                Console.WriteLine(mansion.Mortgage);  // 250000

                // An upcast creates a base class reference from a subclass reference:

                Stock msft2 = new Stock();
                Asset a = msft2;                 // Upcast

                // After the upcast, the two variables still references the same Stock object:

                Console.WriteLine(a == msft2);	// True
            }
            {
                // A downcast operation creates a subclass reference from a base class reference.

                Stock msft = new Stock();
                Asset a = msft;                      // Upcast
                Stock s = (Stock)a;                  // Downcast
                Console.WriteLine(s.SharesOwned);   // <No error>
                Console.WriteLine(s == a);          // True
                Console.WriteLine(s == msft);       // True

                // A downcast requires an explicit cast because it can potentially fail at runtime:

                House h = new House();
                Asset a2 = h;               // Upcast always succeeds
                Stock s2 = (Stock)a2;       // ERROR: Downcast fails: a is not a Stock
            }
            {
                // The is operator tests whether a reference conversion (or unboxing conversion) would succeed:
                Asset a = new Asset();

                if (a is Stock)
                    Console.WriteLine(((Stock)a).SharesOwned);
            }
            {
                // The is operator tests whether a reference conversion(or unboxing conversion) would succeed:

                Asset a = new Stock { SharesOwned = 3 };

                if (a is Stock s)
                    Console.WriteLine(s.SharesOwned);

                // We can take this further:

                if (a is Stock s2 && s2.SharesOwned > 100000)
                    Console.WriteLine("Wealthy");
                else
                    s2 = new Stock();   // s is in scope

                Console.WriteLine(s2.SharesOwned);  // Still in scope
            }
            {
                // The as operator performs a downcast that evaluates to null (rather than throwing an exception)
                // if the downcast fails.

                Asset a = new Asset();
                Stock s = a as Stock;       // s is null; no exception thrown

                if (s != null) Console.WriteLine(s.SharesOwned);    // Nothing written

            }
            #endregion
            Console.ReadLine();

            #region Plolymorphism
            {
                // The Display method below accepts an Asset. This means means we can pass it any subtype:
                Display(new Stock { Name = "MSFT", SharesOwned = 1000 });
                Display(new House { Name = "Mansion", Mortgage = 100000 });
            }
            #endregion
            Console.ReadLine();

            #region Virtual Function
            {
                House mansion = new House { Name = "McMansion", Mortgage = 250000 };
                Console.WriteLine(mansion.Liability);      // 250000
            }
            #endregion
            Console.ReadLine();

            #region Abstract
            {
                Console.WriteLine(new StockA { SharesOwned = 200, CurrentPrice = 123.45M }.NetValue);
            }
            #endregion
            Console.ReadLine();

            #region new vs virtual +++
            {
                Overrider over = new Overrider();
                BaseClass b1 = over;
                over.Foo();                         // Overrider.Foo
                b1.Foo();                           // Overrider.Foo

                Hider h = new Hider();
                BaseClass b2 = h;
                h.Foo();                           // Hider.Foo
                b2.Foo();                          // BaseClass.Foo

                //Seal!

                new Subclass(123);

                new Subclass2();

                FooI(new House());       // Calls Foo (House)

                Asset a = new House();
                FooI(a);             // Calls Foo (Asset)
            }
            #endregion
            Console.ReadLine();

        // Because Stack works with the object type, we can Push and Pop instances of any type to and from the Stack:

            #region Stack + Object
            {
                Stack stack = new Stack();
                stack.Push("sausage");
                stack.Push(1);
                string s = stack.Pop().ToString();   // Downcast, so explicit cast is needed
                Console.WriteLine(s);             // sausage
                Console.WriteLine(stack.Pop().ToString());

                // You can even push value types:
                stack.Push(3);
                int three = (int)stack.Pop();

                // Boxing is the act of casting a value-type instance to a reference-type instance;
                // unboxing is the reverse.
                {
                    int x = 9;
                    object obj = x;           // Box the int
                    int y = (int)obj;         // Unbox the int
                }
                // When unboxing, the types must match exactly:
                {
                    object obj = 9;           // 9 is inferred to be of type int
                    long x = (long)obj;      // InvalidCastException
                }
                {
                    object obj = 9;

                    // First, unbox to the correct type (int), then implicitly convert to long:

                    long x = (int)obj;

                    // This also works:

                    object obj2 = 3.5;              // 3.5 is inferred to be of type double
                    int y = (int)(double)obj2;    // x is now 3
                }
                {
                    // Boxing copies the value-type instance into the new object, and unboxing copies
                    // the contents of the object back into a value-type instance.

                    int i = 3;
                    object boxed = i;
                    i = 5;
                    Console.WriteLine(boxed);    // 3
                }
                {
                    // All types in C# are represented at runtime with an instance of System.Type.
                    // There are two basic ways to get a System.Type object:
                    //  • Call GetType on the instance.
                    //  • Use the typeof operator on a type name.
                    Point p = new Point();
                    Console.WriteLine(p.GetType().Name);             // Point
                    Console.WriteLine(typeof(Point).Name);          // Point
                    Console.WriteLine(p.GetType() == typeof(Point)); // True
                    Console.WriteLine(p.X.GetType().Name);           // Int32
                    Console.WriteLine(p.Y.GetType().FullName);       // System.Int32

                }
            }
            #endregion
            Console.ReadLine();
    
            #region ToString
            {
                // The ToString method is defined on System.Object and returns the default textual representation
                // of a type instance:
                    int x = 1;
                    string s = x.ToString();     // s is "1"

                    PandaStr p = new PandaStr { Name = "Petey" };
                    Console.WriteLine(p.ToString());        // Petey        
            }
            #endregion
            Console.ReadKey();

            #region Struct
            {
                /*// Changing the following struct to a class makes the type legal:

                public struct Point
                {
            	int x = 1;								// Illegal: cannot initialize field
            	int y;
            	public Point() { }						// Illegal: cannot have parameterless constructor	
            	public Point (int x) { this.x = x; }	// Illegal: must assign field y
                }*/
            }
            #endregion
            Console.ReadKey();

            #region Access modifiers
            {
                /*// The access modifiers are public, internal, protected and private.
                 //
                // public is the default for members of an enum or interface.
                // internal is the default for nonnested types.
                // private is the default for everything else.

                class Class1 {} 		// Class1 is internal (default) - visible to other types in same assembly
                public class Class2 {}	// Class2 is visible to everything, including types in other assemblies

                class ClassA
                {
                	int x;				// x is private (default) - cannot be accessed from other types
                }

                class ClassB
                {
            	internal int x;		// x can be accessed from other types in same assembly
                }

                class BaseClass
                {
            	void Foo()           {}    // Foo is private (default)
            	protected void Bar() {}    // Foo is accessible to subclasses
                }

                class Subclass : BaseClass
                {
                   void Test1() { Foo(); }     // Error - cannot access Foo
                   void Test2() { Bar(); }     // OK
                    }

                // A type caps the accessibility of its declared members:

                    class C						// Class C is implicitly internal
                    {
                	public void Foo() {}	// Foo's accessibility is capped at internal
                    }

                // When overriding a base class function, accessibility must be identical on the overridden function:

                class BaseClass             { protected virtual  void Foo() {} }
                class Subclass1 : BaseClass { protected override void Foo() {} }  // OK
                class Subclass2 : BaseClass { public    override void Foo() {} }  // Error

                // A subclass itself can be less accessible than a base class, but not more:

                internal class A { }
                public class B : A { }          // Error
                    */
            }
            #endregion
            Console.ReadKey();
   
            #region interface
            {
                IEnumerator e = new Countdown();
                while (e.MoveNext())
                    Console.Write(e.Current);      // 109876543210	

                {
                    IRedoable r = null;
                    IUndoable u = r;

                    Widget w = new Widget();
                    w.Foo();                      // Widget's implementation of I1.Foo
                    ((I1)w).Foo();                // Widget's implementation of I1.Foo
                    ((I2)w).Foo();                // Widget's implementation of I2.Foo
                }
                {
                    // Calling the interface member through either the base class or the interface
                    // calls the subclass’s implementation:
                    RichTextBox r = new RichTextBox();  // V2.0 --> Calling the reimplemented member through the interface calls the subclass’s implementation:
                    r.Undo();                          // RichTextBox.Undo
                    ((IUndoable)r).Undo();             // RichTextBox.Undo
                    ((TextBox)r).Undo();               // RichTextBox.Undo --> add to RichT.. interface
                }


                // Even with explicit member implementation, interface reimplementation is problematic for a couple of reasons.
                // The following pattern is a good alternative if you need explicit interface implementation -> up!

                S s = new S();
                s.Foo();         // No boxing.

                I i = s;         // Box occurs when casting to interface.
                i.Foo();
            }
            #endregion
            Console.ReadKey();
        mark:
            #region Enums
            {
                BorderSide topSide = BorderSide.Top;
                bool isTop = (topSide == BorderSide.Top);
                Console.WriteLine(isTop);


                // You can convert an enum instance to and from its underlying integral value with an explicit cast:

                int i = (int)BorderSide.Left;
                Console.WriteLine(i + "i");

                BorderSide side = (BorderSide)i;
                Console.WriteLine(side + "side");

                bool leftOrRight = (int)side <= 2;
                Console.WriteLine(leftOrRight + "leftOrRight");

                HorizontalAlignment h = (HorizontalAlignment)BorderSide.Right;
                Console.WriteLine(h + "h");

                BorderSide b = 0;    // No cast required with the 0 literal.
                Console.WriteLine(b + "b");


                BorderSides leftRight = BorderSides.Left | BorderSides.Right;

                if ((leftRight & BorderSides.Left) != 0)
                    Console.WriteLine("Includes Left");   // Includes Left

                string formatted = leftRight.ToString();   // "Left, Right"

                BorderSides s = BorderSides.Left;
                s |= BorderSides.Right;
                Console.WriteLine(s == leftRight);   // True

                s ^= BorderSides.Right;               // Toggles BorderSides.Right
                Console.WriteLine(s);                // Left


                Console.WriteLine(BorderSides2.All);

                // The bitwise, arithmetic, and comparison operators return the result of processing
                // the underlying integral values:	
                Console.WriteLine(BorderSides2.All ^ BorderSides2.LeftRight);


                // Since an enum can be cast to and from its underlying integral type, the actual value
                // it may have may fall outside the bounds of a legal enum member:
                BorderSide b = (BorderSide)12345;
                Console.WriteLine(b);                // 12345

                BorderSide b2 = BorderSide.Bottom;
                b2++;                                   // No errors
                Console.WriteLine(b2);                  // 4 (illegal value)
            }
            {
                for (int i = 0; i <= 16; i++)
                {
                    BorderSides side = (BorderSides)i;
                    Console.WriteLine(IsFlagDefined(side) + " " + side);
                }
            }
            #endregion
            Console.ReadKey();

            #region Nested time
            {
                TopLevel.Color color = TopLevel.Color.Red;

                TopLevel2.Nested.Foo();
            }
            #endregion
            Console.ReadKey();

            #region Generics -> next Lesson ;)
            {

            }
            #endregion
            Console.ReadKey();
        }
    }
}
