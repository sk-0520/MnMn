﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MnMnTest.Logic.Extensions
{
    [TestClass]
    public class LinqExtensionsTest
    {
        [TestMethod]
        public void ToEvaluatedSequenceTest()
        {
            var src = new[] { 1, 2, 3 };
            var list = src.ToEvaluatedSequence();
            Assert.IsTrue(list.SequenceEqual(src));
        }
    }
}
