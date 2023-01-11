using System;
using System.Collections;
using BO;

namespace PL;


internal class Categorys : IEnumerable
{
      static readonly  IEnumerator Category = Enum.GetValues(typeof(Category)).GetEnumerator();
      public  IEnumerator GetEnumerator() => Category;
}