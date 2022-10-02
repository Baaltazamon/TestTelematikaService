﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTelematikaService.Models;

namespace TestTelematikaService.Infrastructure.Interfaces
{
    public interface ICassetteService
    {
        /// <summary>
        /// Получение списка всех кассет
        /// </summary>
        /// <returns></returns>
        IEnumerable<CassetteModel> GetAllCassettes();

        /// <summary>
        /// Создание кассет
        /// </summary>
        /// <param name="count">Количество кассет</param>
        /// <returns></returns>
        List<CassetteModel> CreateCassette(int count);

        /// <summary>
        ///  Проверка, можно ли выдать клиенту указанную сумму купюрами, находящимися в кассетах
        /// </summary>
        /// <param name="amount">Запрашиваемая сумма</param>
        /// <returns></returns>
        bool IssueBanknotes(int amount);

        /// <summary>
        /// Получение списка номиналов банкнот
        /// </summary>
        /// <returns></returns>
        IEnumerable<NominalModel> GetAllNominal();

        /// <summary>
        /// Создание списка кассет
        /// </summary>
        /// <param name="count">Количество кассет</param>
        public void CreateListCassette(int count);

        public void Edit(CassetteModel model);
    }
}
