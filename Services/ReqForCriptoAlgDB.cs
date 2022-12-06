using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class ReqForCriptoAlgDB
    {
        private readonly CriptoAlgorithmsDBContext db;
        public ReqForCriptoAlgDB()
        {
            this.db = new();
            this.db.tableOfCharacteristics.Include(el => el.NumberOfKeys).Include(el => el.InformProcess).Include(el => el.Mode).ToList();
            this.db.cryptoAlgorithmsTables.Include(el => el.TableOfCharacteristics).ToList();
        }

        public IEnumerable<object[]> GetListOfCryptoAlgorithms()
        {
            if (this.db.cryptoAlgorithmsTables == null) { throw new Exception($"Ошибка бд. Пустая ссылка на таблицу"); }
            return this.db.cryptoAlgorithmsTables.Select(el => new object[4] { false, el.Id, el.Title, el.NameOfAlg });
        }
        public IEnumerable<object[]> GetListOfCryptoAlgorithmsMode(IEnumerable<int> id)
        {
            if (this.db.cryptoAlgorithmsTables == null) { throw new Exception($"Ошибка бд. Пустая ссылка на таблицу"); }
            return this.db.cryptoAlgorithmsTables
                .Where(el=>id.Contains(el.Id) == true)
                .Select(el => new object[4] 
                { 
                    false, 
                    el.TableOfCharacteristics.Mode.Id, 
                    el.TableOfCharacteristics.Mode.Title,
                    el.TableOfCharacteristics.Mode.NameOfMode
                }).Distinct();
        }
        public IEnumerable<object[]> GetModeForId(int id)
        {
            return this.db.replacementModes.Where(el => el.Id == id).Select(el => new object[2] { el.Id, el.Title });
        }
    }
}
