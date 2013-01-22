using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Task.Utils
{
    public static class Registry //класс для хранения глобальных переменных в хеш-таблице
    {
        public static Hashtable hashTable = new Hashtable();
    }
}
