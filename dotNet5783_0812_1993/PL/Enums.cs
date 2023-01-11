using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using BO;
namespace PL;


internal class Categorys : IEnumerable
{
      static readonly  IEnumerator Category = Enum.GetValues(typeof(Category)).GetEnumerator();
      public  IEnumerator GetEnumerator() => Category;
}