﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.1.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace QingStor_SDK_CSharp_Test.TestFeature
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("the object multipart feature")]
    [NUnit.Framework.CategoryAttribute("object_multipart")]
    public partial class TheObjectMultipartFeatureFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "object_multipart.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "the object multipart feature", null, ProgrammingLanguage.CSharp, new string[] {
                        "object_multipart"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("the object multipart feature")]
        [NUnit.Framework.TestCaseAttribute("test_object_multipart", new string[0])]
        [NUnit.Framework.TestCaseAttribute("中文分块测试", new string[0])]
        [NUnit.Framework.TestCaseAttribute("田中さんにあげて下さいブロック", new string[0])]
        public virtual void TheObjectMultipartFeature(string key, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("the object multipart feature", exampleTags);
#line 4
  this.ScenarioSetup(scenarioInfo);
#line 6
    testRunner.When(string.Format("initiate multipart upload with key \"{0}\"", key), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 7
    testRunner.Then("initiate multipart upload status code is 200", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 10
    testRunner.When(string.Format("upload the first part with key \"{0}\"", key), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 11
    testRunner.Then("upload the first part status code is 201", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 12
    testRunner.When(string.Format("upload the second part with key \"{0}\"", key), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 13
    testRunner.Then("upload the second part status code is 201", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 14
    testRunner.When(string.Format("upload the third part with key \"{0}\"", key), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 15
    testRunner.Then("upload the third part status code is 201", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 18
    testRunner.When(string.Format("list multipart with key \"{0}\"", key), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 19
    testRunner.Then("list multipart status code is 200", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 20
    testRunner.And("list multipart object parts count is 3", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
    testRunner.When(string.Format("complete multipart upload with key \"{0}\"", key), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 24
    testRunner.Then("complete multipart upload status code is 201", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 27
    testRunner.When(string.Format("abort multipart upload with key \"{0}\"", key), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 28
    testRunner.Then("abort multipart upload status code is 400", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 31
    testRunner.When(string.Format("delete the multipart object with key \"{0}\"", key), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 32
    testRunner.Then("delete the multipart object status code is 204", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
