﻿using Business.Abstract;
using Business.Constraints;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Businness;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entity.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class BrandManager : IBrandService
    {

        IBrandDal _brandDal;

        public BrandManager(IBrandDal brandDal)
        {
            _brandDal = brandDal;
        }

        [ValidationAspect(typeof(BrandValidator))]
        public IResult Add(Brand brand)
        {

           IResult result= BusinnessRules.Run(CheckIfBrandExists(brand));
            if (result!=null)
            {
                return result;

            }

            _brandDal.Add(brand);
            return new SuccessResult(Messages.BrandAdded);

            


        }

        public IResult Delete(Brand brand)
        {
            try
            {
                _brandDal.Delete(brand);
                return new SuccessResult(Messages.BrandDeleted);
            }
            catch (Exception)
            {

                return new ErrorResult(Messages.BrandCantDeledet);
            }

        }

        public IDataResult<List<Brand>> GetAll()
        {
            return new SuccessDataResult<List<Brand>>(_brandDal.GetAll(), Messages.BrandsListed);
        }

        public IDataResult<Brand> GetById(int id)
        {
            return new SuccessDataResult<Brand>(_brandDal.Get(brand => brand.BrandID == id), Messages.BrandsListed);
        }

        public IResult Update(Brand brand)
        {
            try
            {
                _brandDal.Update(brand);
                return new SuccessResult(Messages.BrandUpdated);
            }
            catch (Exception)
            {

                return new ErrorResult(Messages.BrandCantUpdated);
            }

        }
        private IResult CheckIfBrandExists(Brand brand)
        {
            var result = _brandDal.GetAll(p => p.BrandName == brand.BrandName).Count;
            if (result == 0)
            {
                return new SuccessResult(Messages.BrandAdded);
            }
            return new ErrorResult(Messages.BrandNameAlreadyExist);

        }
    }
}
