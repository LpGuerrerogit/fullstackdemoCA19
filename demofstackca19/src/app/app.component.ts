import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  nombre = '';
  edad = 0;
  safeImageSrc: SafeUrl = '';
  sasUrl = '';

  constructor(private http: HttpClient, private sanitizer: DomSanitizer) {}

  upload() {
    const data = { nombre: this.nombre, edad: this.edad };
    this.http.post('/api/datos/insert', data).subscribe({
      next: () => alert('Datos enviados correctamente'),
      error: err => alert('Error al enviar datos: ' + err.message)
    });
  }

  descargar() {
    this.http.get('/api/datos/downloadBase64', { responseType: 'text' }).subscribe({
      next: (res) => {
        this.safeImageSrc = res; 
      },
      error: err => alert('Error al descargar imagen: ' + err.message)
    });
  }

  getSas() {
    this.http.get('/api/datos/sas', { responseType: 'text' }).subscribe({
      next: (res) => this.sasUrl = res,
      error: err => alert('Error al obtener SAS: ' + err.message)
    });
  }
}
