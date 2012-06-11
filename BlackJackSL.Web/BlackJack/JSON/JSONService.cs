using System.Collections.Generic;

namespace BlackJackSL.Web.BlackJack.JSON
{
    public class JSONService : BlackJackSL.Web.BlackJack.JSON.IJSONService
    {
        public SampleDataObject GetSingleObject(int id, string name)
        {
            return new SampleDataObject(id, name);
        }

        public ICollection<SampleDataObject> GetMultipleObjects(int number)
        {
            List<SampleDataObject> list = new List<SampleDataObject>();
            for (int i = 0; i < number; i++)
            {
                list.Add(new SampleDataObject(i, "Name" + i));
            }
            return list;
        }
    }
}