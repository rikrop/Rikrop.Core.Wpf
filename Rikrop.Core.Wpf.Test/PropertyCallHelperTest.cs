using System;
using NUnit.Framework;

namespace Rikrop.Core.Wpf.Test
{
    [TestFixture]
    public class PropertyCallHelperTest
    {
        private class BigModel
        {
            private readonly SmallModel _smallModel = new SmallModel();

            public SmallModel SmallModelProperty
            {
                get { return _smallModel; }
            }

            public bool BoolProperty
            {
                get { throw new NotImplementedException(); }
            }

            public PropertyCall GetThisPropertyCall()
            {
                return PropertyCallHelper.GetPropertyCall(() => SmallModelProperty.StringProperty);
            }

            public PropertyCall GetThisBoolPropertyCall()
            {
                return PropertyCallHelper.GetPropertyCall(() => BoolProperty);
            }
        }

        private class SmallModel
        {
            public string StringProperty
            {
                get { throw new NotImplementedException(); }
            }

            public bool BoolProperty
            {
                get { throw new NotImplementedException(); }
            }

            public PropertyCall GetThisPropertyCall()
            {
                return PropertyCallHelper.GetPropertyCall(() => StringProperty);
            }

            public PropertyCall GetThisBoolPropertyCall()
            {
                return PropertyCallHelper.GetPropertyCall(() => BoolProperty);
            }
        }

        [Test]
        public void ShouldWorkWithDeepRefTypePropertyCallOnAnotherObject()
        {
            var bigModel = new BigModel();
            var result = PropertyCallHelper.GetPropertyCall(() => bigModel.SmallModelProperty.StringProperty);

            Assert.AreEqual(bigModel.SmallModelProperty, result.TargetObject);
            Assert.AreEqual("StringProperty", result.TargetPropertyName);
        }

        [Test]
        public void ShouldWorkWithDeepRefTypePropertyCallOnThis()
        {
            var bigModel = new BigModel();
            var result = bigModel.GetThisPropertyCall();

            Assert.AreEqual(bigModel.SmallModelProperty, result.TargetObject);
            Assert.AreEqual("StringProperty", result.TargetPropertyName);
        }

        [Test]
        public void ShouldWorkWithDeepValueTypePropertyCallOnAnotherObject()
        {
            var bigModel = new BigModel();
            var result = PropertyCallHelper.GetPropertyCall(() => bigModel.SmallModelProperty.BoolProperty);

            Assert.AreEqual(bigModel.SmallModelProperty, result.TargetObject);
            Assert.AreEqual("BoolProperty", result.TargetPropertyName);
        }

        [Test]
        public void ShouldWorkWithDeepValueTypePropertyCallOnThis()
        {
            var bigModel = new BigModel();
            var result = bigModel.SmallModelProperty.GetThisBoolPropertyCall();

            Assert.AreEqual(bigModel.SmallModelProperty, result.TargetObject);
            Assert.AreEqual("BoolProperty", result.TargetPropertyName);
        }

        [Test]
        public void ShouldWorkWithSimpleRefTypePropertyCallOnAnotherObject()
        {
            var bigModel = new BigModel();
            var result = PropertyCallHelper.GetPropertyCall(() => bigModel.SmallModelProperty);

            Assert.AreEqual(bigModel, result.TargetObject);
            Assert.AreEqual("SmallModelProperty", result.TargetPropertyName);
        }

        [Test]
        public void ShouldWorkWithSimpleRefTypePropertyCallOnThis()
        {
            var smallModel = new SmallModel();
            var result = smallModel.GetThisPropertyCall();

            Assert.AreEqual(smallModel, result.TargetObject);
            Assert.AreEqual("StringProperty", result.TargetPropertyName);
        }

        [Test]
        public void ShouldWorkWithSimpleValueTypePropertyCallOnAnotherObject()
        {
            var bigModel = new BigModel();
            var result = PropertyCallHelper.GetPropertyCall(() => bigModel.BoolProperty);

            Assert.AreEqual(bigModel, result.TargetObject);
            Assert.AreEqual("BoolProperty", result.TargetPropertyName);
        }

        [Test]
        public void ShouldWorkWithSimpleValueTypePropertyCallOnThis()
        {
            var bigModel = new BigModel();
            var result = bigModel.GetThisBoolPropertyCall();

            Assert.AreEqual(bigModel, result.TargetObject);
            Assert.AreEqual("BoolProperty", result.TargetPropertyName);
        }
    }
}