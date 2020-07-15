using System;
using System.Collections.Generic;
using System.Text;

namespace hashes
{
    public class GhostsTask : 
		IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>, 
		IMagic
	{
        private static Dictionary<Type, Action<object>> typeActionDict;
        private object obj;
        private static byte[] documentContent;
        private static Vector segmentVector;
        private static Cat cat;
        static GhostsTask()
        {
            documentContent = new byte[] { 1, 2, 3 };
            segmentVector = new Vector(10, 10);
            cat = new Cat("Tom", "yes", DateTime.MaxValue);

            typeActionDict = new Dictionary<Type, Action<object>> {
              { typeof(Vector), (object obj) => obj = ((Vector)obj).Add(new Vector(100.0, -1.0)) },
              { typeof(Segment), (object obj) => segmentVector.Add(segmentVector) },
              { typeof(Cat), (object obj) =>  cat.Rename("Jerry") },
              { typeof(Robot), (object obj) => obj = Robot.BatteryCapacity = 50 },
              { typeof(Document), (object obj) => documentContent[0] = 6 },
            };
        }

        
        public void DoMagic()
		{
            typeActionDict[obj.GetType()](obj);
		}

		// Чтобы класс одновременно реализовывал интерфейсы IFactory<A> и IFactory<B> 
		// придется воспользоваться так называемой явной реализацией интерфейса.
		// Чтобы отличать методы создания A и B у каждого метода Create нужно явно указать, к какому интерфейсу он относится.
		// На самом деле такое вы уже видели, когда реализовывали IEnumerable<T>.

		Vector IFactory<Vector>.Create()
		{
            obj = segmentVector;
            return segmentVector;
        }

		Segment IFactory<Segment>.Create()
		{
            var seg = new Segment(new Vector(0.0, 0.0), segmentVector);
            obj = seg;
            return seg;
		}

        Cat IFactory<Cat>.Create()
        {
            obj = cat;
            return cat;
        }

        Robot IFactory<Robot>.Create()
        {
            var robot = new Robot("1", 1.0);
            obj = robot;
            return robot;
        }

        Document IFactory<Document>.Create()
        {
            var doc = new Document("Fahrenheit 451", System.Text.Encoding.UTF8, documentContent);
            obj = doc;
            return doc;
        }
    }
}