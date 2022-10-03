using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTelematikaService.Infrastructure.Interfaces;
using TestTelematikaService.Models;

namespace TestTelematikaService.Infrastructure.Services
{
    public class InMemoryCasseteService: ICassetteService
    {
        private  List<CassetteModel> _cassettes;
        private readonly List<NominalModel> _nominal;
        private readonly List<CassetteModel> _issueBanknote;
        private int[] Log;
        

        public InMemoryCasseteService()
        {
            _nominal = new List<NominalModel>
            {
                new NominalModel
                {
                    Id = 1,
                    NominalValue = 100
                },
                new NominalModel
                {
                    Id = 2,
                    NominalValue = 200
                },
                new NominalModel
                {
                    Id = 3,
                    NominalValue = 500
                },
                new NominalModel
                {
                    Id = 4,
                    NominalValue = 1000
                },
                new NominalModel
                {
                    Id = 5,
                    NominalValue = 2000
                },
                new NominalModel
                {
                    Id = 6,
                    NominalValue = 5000
                }
            };
            _cassettes = new List<CassetteModel>();
            _issueBanknote = new List<CassetteModel>();
        }

        public IEnumerable<CassetteModel> GetAllCassettes()
        {
            return _cassettes;
        }

        public void CreateListCassette(int count)
        {
            _cassettes = CreateCassette(count);
        }

        public void Edit(CassetteModel model)
        {
            var cassette = _cassettes.SingleOrDefault(c => c.Id == model.Id);
            if (cassette is null)
                return;
            cassette.Quantity = model.Quantity;
            cassette.Serviceable = model.Serviceable;
            cassette.NominalValue = model.NominalValue;
        }

        public void IssueBanknote(int id, int count, NominalModel nominal)
        {
            CassetteModel cassetteModel = new CassetteModel
            {
                Id = id,
                NominalValue = nominal,
                Quantity = count,
                Serviceable = true
            };
            _issueBanknote.Add(cassetteModel);
        }

        public List<CassetteModel> GetIssueBanknotes()
        {
            return _issueBanknote;
        }

        public List<CassetteModel> CreateCassette(int count)
        {
            CassetteModel cassette;
            List<CassetteModel> cassettes = new List<CassetteModel>();
            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {

                cassette = new CassetteModel
                {
                    NominalValue = _nominal[rnd.Next(0, 6)],
                    Id = i + 1,
                    Quantity = rnd.Next(1, 100),
                    Serviceable = Convert.ToBoolean(rnd.Next(0, 2))
                };
                cassettes.Add(cassette);
            }

            return cassettes;
        }

        

        public bool IssueBanknotes(int amount)
        {



            #region oldMethod


            _cassettes = _cassettes.OrderByDescending(c => c.NominalValue.NominalValue).ToList();
            List<CassetteModel> cassettes = _cassettes.Where(c => c.NominalValue.NominalValue <= amount).ToList();
            Log = new int[cassettes.Count];
            _issueBanknote.Clear();

            amount = Culculate(amount, cassettes);

            if (amount > 0)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < Log.Length; i++)
                {
                    if (Log[i] > 0)
                        IssueBanknote(cassettes[i].Id, Log[i], cassettes[i].NominalValue);

                }
                return true;
            }

            #endregion

        }

        private int Culculate(int amount, List<CassetteModel> cassettes)
        {
            int count = 0;
            int deg = 0;
            bool change = false;
            int contAmount = 0;
            bool first = true;
            do
            {
                contAmount = amount;
                for (int i = 0; i < cassettes.Count; i++)
                {
                    if (_cassettes[i].Serviceable.Equals(false))
                    {
                        Log[i] = 0;
                        continue;
                    }
                    deg = 0;
                    if (CheckLog() && !first)
                    {
                        if (Log[i] == 0 && !change)
                        {
                            
                            continue;
                        }
                        if (Log[i] > 0 && !change)
                        {
                            deg = 1;
                            change = true;
                        }
                        
                    }
                    count = contAmount / cassettes[i].NominalValue.NominalValue;
                    count -= (count - (Log[i]-1)) * deg;
                    if (count > cassettes[i].Quantity)
                    {
                        contAmount -= cassettes[i].Quantity * cassettes[i].NominalValue.NominalValue;
                        Log[i] = cassettes[i].Quantity;
                        continue;
                    }
                    contAmount -= count * cassettes[i].NominalValue.NominalValue;
                    Log[i] = count;
                }

                change = false;
                first = false;
            } while (CheckLog() && contAmount > 0);
            

            return contAmount;
        }
       

        private bool CheckLog()
        {
            bool check = false;
            for (int i = 0; i < Log.Length; i++)
            {
                if (Log[i] > 0)
                {
                    return true;
                }
                    
            }

            return false;
        }
        public IEnumerable<NominalModel> GetAllNominal()
        {
            return _nominal;
        }
    }
}
