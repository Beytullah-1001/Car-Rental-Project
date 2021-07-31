﻿using Business.Abstract;
using Business.Constraints;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class RentalManager : IRentalService
    {
        IRentalDal _rentalDal;
        

        public RentalManager(IRentalDal rentalDal)
        {
            _rentalDal = rentalDal;
        }


        public IResult Add(Rental rental)
        {
            IResult result = BusinessRules.Run(
                IsThatCarDeliveried(rental.CarID));

            if (result != null)
            {
                return result;
            }

            
           _rentalDal.Add(rental);
           return new SuccessResult(Messages.RentalAdded);
            

        }

        public IResult Delete(Rental rental)
        {
            try
            {
                _rentalDal.Delete(rental);
                return new SuccessResult(Messages.RentalDeleted);
            }
            catch (Exception)
            {

                return new ErrorResult(Messages.RentalCantDeledet);
            }
        }

        public IDataResult<List<Rental>> GetAll()
        {
            return new SuccessDataResult<List<Rental>>(_rentalDal.GetAll(), Messages.RentalsListed);
        }

        public IDataResult<Rental> GetByCustomerId(int id)
        {
            return new SuccessDataResult<Rental>(_rentalDal.Get(rental => rental.CustomerID == id), Messages.RentalListed);
        }

        public IResult Update(Rental rental)
        {
            try
            {
                _rentalDal.Update(rental);
                return new SuccessResult(Messages.RentalUpdated);
            }
            catch (Exception)
            {

                return new ErrorResult(Messages.RentalCantUpdated);
            }
        }
        public IResult IsThatCarDeliveried(int id)
        {
            var result = _rentalDal.Get(r => r.CarID ==id && r.ReturnDate==null); // parametre olarak aldıgın id ye göre rentali getir. ve gelen kayıt'ın returndate i boş ise getir
            if (result == null)
            {
                return new SuccessResult(); // demek ki hiçbir ilan gelmemiş, demek ki return tarihi boş değil yani araç teslim edilmiş.
            }
            return new ErrorResult("Araba Teslim Edilmemiş bu şartlar altında araba kiralanamaz");
        }
    }
}
