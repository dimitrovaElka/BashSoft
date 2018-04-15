using BashSoft.Contracts;
using BashSoft.DataStuctures;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BashSoftTesting
{
    [TestFixture]
    public class OrderedDataStructureTests
    {
        private ISimpleOrderedBag<string> names;

        [Test]
        public void TestEmptyCtor()
        {
         //   this.names = new SimpleSortedList<string>();
            Assert.That(this.names.Capacity, Is.EqualTo(16));
            Assert.That(this.names.Size, Is.EqualTo(0));
        }

        [Test]
        public void TestCtorWithInitialCapacity()
        {
            this.names = new SimpleSortedList<string>(20);
            Assert.That(this.names.Capacity, Is.EqualTo(20));
            Assert.That(this.names.Size, Is.EqualTo(0));
        }

        [Test]
        public void TestCtorWithAllParams()
        {
            this.names = new SimpleSortedList<string>(StringComparer.OrdinalIgnoreCase, 30);
            Assert.That(this.names.Capacity, Is.EqualTo(30));
            Assert.That(this.names.Size, Is.EqualTo(0));
        }

        [Test]
        public void TestCtorWithInitialComparer()
        {
            this.names = new SimpleSortedList<string>(StringComparer.OrdinalIgnoreCase);
            Assert.That(this.names.Capacity, Is.EqualTo(16));
            Assert.That(this.names.Size, Is.EqualTo(0));
        }

        [SetUp]
        public void SetUp()
        {
            this.names = new SimpleSortedList<string>();
        }

        [Test]
        public void TestAddIncreasesSize()
        {
            this.names.Add("Nasko");
            Assert.That(this.names.Size, Is.EqualTo(1));
        }

        [Test]
        public void TestAddNullThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => this.names.Add(null));
        }

        [Test]
        public void TestAddUnsortedDataIsHeldSorted()
        {
            this.names.Add("Rosen");
            this.names.Add("Georgi");
            this.names.Add("Balkan");
            List<string> sortedNames = new List<string>()
            {
                "Balkan", "Georgi", "Rosen"
            };
            Assert.That(this.names, Is.EquivalentTo(sortedNames));
        }

        [Test]
        public void TestAddingMoreThanInitialCapacity()
        {
            for (int i = 0; i < 17; i++)
            {
                this.names.Add(i.ToString());
            }
            Assert.That(this.names.Size, Is.EqualTo(17));
            Assert.AreNotEqual(this.names.Capacity, 16);
        }

        [Test]
        public void TestAddingAllFromCollectionIncreasesSize()
        {
            List<string> listToAdd = new List<string>()
            {
                "one", "two"
            };
            this.names.AddAll(listToAdd);
            Assert.That(this.names.Size, Is.EqualTo(2));
        }

        [Test]
        public void TestAddingAllFromNullThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => this.names.AddAll(null));
        }

        [Test]
        public void TestAddAllKeepsSorted()
        {
            List<string> addedNames = new List<string>()
            {
                 "Georgi", "Balkan", "Rosen"
            };
            this.names.AddAll(addedNames); 
            List<string> sortedNames = new List<string>()
            {
                "Balkan", "Georgi", "Rosen"
            };
            Assert.That(this.names, Is.EquivalentTo(sortedNames));
        }

        [Test]
        public void TestRemoveValidElementDecreasesSize()
        {
            this.names.Add("element");
            this.names.Remove("element");
            Assert.That(this.names.Size, Is.EqualTo(0));
        }

        [Test]
        public void TestRemoveValidElementRemovesSelectedOne()
        {
            this.names.Add("Ivan");
            this.names.Add("Nasko");
            this.names.Remove("Ivan");
            Assert.That(this.names, !Contains.Item("Ivan"));
        }

        [Test]
        public void TestRemovingNullThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => this.names.Remove(null));
        }

        [Test]
        public void TestJoinWithNull()
        {
            Assert.Throws<ArgumentNullException>(() => this.names.JoinWith(null));
        }

        [Test]
        public void TestJoinWorksFine()
        {
            this.names.Add("Ivan");
            this.names.Add("Nasko");
            string expected = "Ivan, Nasko";
            Assert.That(this.names.JoinWith(", "), Is.EqualTo(expected));
        }
    }
}
