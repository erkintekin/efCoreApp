using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using efCoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace efCoreApp.Controllers
{

    public class OgrenciController : Controller
    {
        // Önemli
        private readonly DataContext _context;

        public OgrenciController(DataContext context)
        {
            // Dependency Injection - Veritabanına bağlılığı en aza indirmek. Yeni nesne devamlı üretmemek. Mevcut nesneyi referans olarak çağırmak.
            _context = context;
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ogrenci? model)
        {
            _context.Ogrenciler.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index"); // Index sayfasına HomeController gönderdik. Buradaki Index boş çünkü.
        }

        public async Task<IActionResult> Index()
        {
            var ogrenciler = await _context.Ogrenciler.ToListAsync();

            return View(ogrenciler);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Ogrenci? selectedStudent = await _context.Ogrenciler.FirstOrDefaultAsync(s => s.OgrenciId == id);
            _context.Ogrenciler.Remove(selectedStudent);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            Ogrenci? selectedStudent = await _context.Ogrenciler.FirstOrDefaultAsync(s => s.OgrenciId == id);

            // if (selectedStudent == null)
            // {
            //     return NotFound(); // Eğer öğrenci bulunamazsa 404 hatası döndür
            // }

            return View(selectedStudent);


        }

        [HttpPost]
        public async Task<IActionResult> Edit(Ogrenci? model)
        {

            Ogrenci? selectedStudent = await _context.Ogrenciler.FirstOrDefaultAsync(s => s.OgrenciId == model.OgrenciId);

            if (selectedStudent == null)
            {
                return NotFound();
            }

            model.OgrenciAd = selectedStudent.OgrenciAd;
            model.OgrenciSoyad = selectedStudent.OgrenciSoyad;
            model.Eposta = selectedStudent.Eposta;
            model.Telefon = selectedStudent.Telefon;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Ogrenci");
        }
    }
}