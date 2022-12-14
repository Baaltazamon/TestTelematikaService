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

            _cassettes = _cassettes.OrderByDescending(c => c.NominalValue.NominalValue).ToList();
            _issueBanknote.Clear();
            int count = 0;

            for (int i = 0; i < _cassettes.Count; i++)
            {
                if (_cassettes[i].Serviceable.Equals(false) || amount < _cassettes[i].NominalValue.NominalValue)
                {
                    continue;
                }
                count = amount / _cassettes[i].NominalValue.NominalValue;
                if (count > _cassettes[i].Quantity)
                {
                    amount -= _cassettes[i].Quantity * _cassettes[i].NominalValue.NominalValue;
                    IssueBanknote(_cassettes[i].Id, _cassettes[i].Quantity, _cassettes[i].NominalValue);
                    continue;
                }
                amount -= count * _cassettes[i].NominalValue.NominalValue;
                IssueBanknote(_cassettes[i].Id, count, _cassettes[i].NominalValue);
            }


            if (amount > 0)
                return false;
            else
                return true;
        }


        public IEnumerable<NominalModel> GetAllNominal()
        {
            return _nominal;
        }
    }
}
