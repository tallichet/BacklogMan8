using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Md5.Test
{
    [TestClass]
    public class Md5Test
    {
        private static Tuple<T1, T2> Pair<T1, T2>(T1 t1, T2 t2)
        {
            return Tuple.Create(t1, t2);
        }

        public IEnumerable<Tuple<string, string>> TestData
        {
            get
            {
                yield return Pair("51bb50d72b1fc", "25E584BC1E1DAC32D2EFF83E942AAE79");
                yield return Pair("51bb50d72d91d", "697AA8744801AF070A0CA8C21363E0C4");
                yield return Pair("51bb50d73005c", "EAFFDEECBDFBB060A0CD10F74FDFD28E");
                yield return Pair("51bb50d732767", "85B533FCA96A5D6718653DFDAF629B98");
                yield return Pair("51bb50d734e90", "54E1FFDAAAA9FF2D46165948FB623148");
                yield return Pair("51bb50d7375b2", "5282FB4ADB47C0382F14198690217803");
                yield return Pair("51bb50d739cda", "FCE99B1DE667370F0743C135F2EB3CB1");
                yield return Pair("51bb50d73c410", "3C4489BA48FCD4232468AFED3A63260F");
                yield return Pair("51bb50d73eb3b", "69AC6A64C7B9428983A5676FDC341766");
                yield return Pair("51bb50d74125c", "CD8FC6346E0EFDE2C2426CDCF9D383C8");
                yield return Pair("51bb50d743984", "70A6A835B750C2996248E3F0398F8767");
                yield return Pair("51bb50d7460a6", "3DCC41139F05190368CFEF79720A5BEE");
                yield return Pair("51bb50d7487ce", "6FC80730B8800EE6BBB093B89726EC8C");
                yield return Pair("51bb50d74aef1", "09DD7021176A0A229EFCBE624D80D746");
                yield return Pair("51bb50d74d619", "6133BA3E879319A7D29B22004CEBF6CC");
                yield return Pair("51bb50d74fd4f", "95CB3B1CC026B14E5D182158D93FD44C");
                yield return Pair("51bb50d752477", "6EA2315CE927E90341A84AB46E7A7BC7");
                yield return Pair("51bb50d754b9a", "2DCBE2359BB8885645907900E927AE04");
                yield return Pair("51bb50d7572d6", "6F6A8EFA686135D372440AB7EFC32D97");
                yield return Pair("51bb50d7599f9", "97E99560859CEA40E00990D2385792DC");
            }
        }

        public IEnumerable<Tuple<string, string>> LongTestData
        {
            get
            {
                yield return Pair("Lorem ipsum dolor sit amet, consectetur adipisicing elit, " +
                                  "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                                  "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris " +
                                  "nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in " +
                                  "reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
                                  "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui " +
                                  "officia deserunt mollit anim id est laborum.",
                                  "FA5C89F3C88B81BFD5E821B0316569AF");

                yield return Pair("Sed ut perspiciatis unde omnis iste natus error sit voluptatem " +
                                  "accusantium doloremque laudantium, totam rem aperiam, eaque ipsa " +
                                  "quae ab illo inventore veritatis et quasi architecto beatae vitae " +
                                  "dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit " +
                                  "aspernatur aut odit aut fugit, sed quia consequuntur magni dolores " +
                                  "eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, " +
                                  "qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, " +
                                  "sed quia non numquam eius modi tempora incidunt ut labore et dolore " +
                                  "magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis " +
                                  "nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut " +
                                  "aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit " +
                                  "qui in ea voluptate velit esse quam nihil molestiae consequatur, " +
                                  "vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?", 
                                  "093402DC785C01274713767FB5962628");

                yield return Pair("But I must explain to you how all this mistaken idea of denouncing pleasure " +
                                  "and praising pain was born and I will give you a complete account of the system, " +
                                  "and expound the actual teachings of the great explorer of the truth, " +
                                  "the master-builder of human happiness. No one rejects, dislikes, " +
                                  "or avoids pleasure itself, because it is pleasure, but because those who " +
                                  "do not know how to pursue pleasure rationally encounter consequences that " +
                                  "are extremely painful. Nor again is there anyone who loves or pursues or desires " +
                                  "to obtain pain of itself, because it is pain, but because occasionally circumstances " +
                                  "occur in which toil and pain can procure him some great pleasure. " +
                                  "To take a trivial example, which of us ever undertakes laborious physical exercise, " +
                                  "except to obtain some advantage from it? But who has any right to find fault " +
                                  "with a man who chooses to enjoy a pleasure that has no annoying consequences, " +
                                  "or one who avoids a pain that produces no resultant pleasure?",
                                   "866D7BA05AF962C4D2E20D86375C2402");
                yield return Pair("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "3B0C8AC703F828B04C6C197006D17218");
            }
        }

        [TestMethod]
        public void Test()
        {
            foreach (var tuple in TestData)
            {
                Assert.AreEqual(tuple.Item2, MD5Core.GetHashString(tuple.Item1));
            }
        }

        [TestMethod]
        public void TestLong()
        {
            foreach (var tuple in LongTestData)
            {
                Assert.AreEqual(tuple.Item2, MD5Core.GetHashString(tuple.Item1));
            }
        }
    }
}
