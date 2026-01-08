import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common'; // Dodajemy to dla stylów dynamicznych

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('front_app');

  powitanie = 'Witaj w nowym Angularze';
  licznik = 0;

  zwieksz() {
    this.licznik++;
  }

  // 1. Tworzymy tablicę z początkowymi zadaniami
  listaZadan = [
    { tekst: 'Nauczyć się podstaw Angulara', gotowe: false }, 
    { tekst: 'Zrobić pierwszą stronę', gotowe: true}
  ];

  // 2. Funkcja dodająca nowe zadanie do listy
  dodajZadanie(pole: HTMLInputElement) {
    if (pole.value.trim() !== '') {
      this.listaZadan.push({ tekst: pole.value, gotowe : false});
      pole.value = ''; // Czyścimy pole po dodaniu
    }
  }

  usunZadanie(indeks: number) {
  // .splice(od_którego_miejsca, ile_elementów_usunąć)
  this.listaZadan.splice(indeks, 1);
  }

  przelaczStatus(indeks: number) {
    this.listaZadan[indeks].gotowe = !this.listaZadan[indeks].gotowe
  }

  get pozostaloZadan() {
    // .filter tworzy nową listę tylko z nieukończonymi zadaniami
    // .length zwraca ich liczbę 
    return this.listaZadan.filter(z => !z.gotowe).length;
  }
}
