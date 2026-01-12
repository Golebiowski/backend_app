import { Component, signal, computed } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common'; // Dodajemy to dla stylów dynamicznych
import { trigger, transition, style, animate } from '@angular/animations';
import { effect } from '@angular/core'; // dodaj do importów

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css',
  animations : [
    trigger('listaAnimacja', [
      transition(':enter', [
      style({ opacity: 0, tansform: 'translateX(-20)' }), //Start: niewidoczne i przesunięte
      animate('300ms ease-out', style({ opacity: 1, transform: 'translateX(0)' })) // Koniec: widoczne
    ]),
    transition(':leave', [
      animate('200ms ease-in', style({opacity: 0, transform: 'scale(0.95)'}))
    ])
    ])
  ]
})
export class App {
  protected readonly title = signal('front_app');

  powitanie = 'Witaj w nowym Angularze';
  licznik = 0;

  constructor() { 
    // 1. ODCZYT: Próbujemy pobrać dane z LocalStorage przy starcie
    const dane = localStorage.getItem('moje_zadania');
    if(dane)
    {
      this.listaZadan.set(JSON.parse(dane));
    }

    effect(() => { 
    localStorage.setItem('moje_zadania', JSON.stringify(this.listaZadan()));
    console.log('Zapisano zmiany w LocalStorage');
  });
  } 

  // Funkcja pomocnicza do zapisu
  //zapiszWLocalStorage() {
  // Pamiętaj o () przy listaZadan!
  //localStorage.setItem('moje_zadania', JSON.stringify(this.listaZadan()));
  //}

  zwieksz() {
    this.licznik++;
  }

  // 1. Tworzymy tablicę z początkowymi zadaniami
  listaZadan = signal([
    { tekst: 'Nauczyć się podstaw Angulara', gotowe: false }, 
    { tekst: 'Zrobić pierwszą stronę', gotowe: true}
  ]);

  aktualnyFiltr = signal<'wszystkie' | 'aktywne' | 'zakonczone'>('wszystkie');

  // 2. Funkcja dodająca nowe zadanie do listy
  dodajZadanie(pole: HTMLInputElement) {
    if (pole.value.trim() !== '') {
      // .update() pozwala nam zmodyfikować obecną wartość sygnału
      this.listaZadan.update(staraLista => [...staraLista, { tekst: pole.value, gotowe : false}]);
      pole.value = ''; // Czyścimy pole po dodaniu
      //this.zapiszWLocalStorage(); // Zapisz po dodaniu - usunięte po dodaniu effect()
    }
  }

  usunZadanie(zadanieDoUsuniecia : any) {
  // .splice(od_którego_miejsca, ile_elementów_usunąć)
  this.listaZadan.update(lista => lista.filter(z => z !== zadanieDoUsuniecia));
  }

  przelaczStatus(zadanie: any) {
    this.listaZadan.update(lista => 
      lista.map(z => z === zadanie ? { ...z, gotowe: !z.gotowe} : z)
    )
    //this.zapiszWLocalStorage(); - usunięte po dodaniu effect()
  }

  pozostaloZadan = computed(() => { 
    const zadania = this.listaZadan();

    return zadania.filter(z => !z.gotowe).length;
  })

  wykonanychZadan = computed(() => { 
    const zadania = this.listaZadan();

    return zadania.filter(z => z.gotowe).length;
  })

  wszystkichZadan = computed(() => { 
    const zadania = this.listaZadan();

    return zadania.length;
  })

  przefiltrowaneZadania = computed(() => {
    const zadania = this.listaZadan();
    const filtr = this.aktualnyFiltr();

    if (filtr === 'aktywne') return zadania.filter(z => !z.gotowe);
    if (filtr === 'zakonczone') return zadania.filter(z => z.gotowe);  
    return zadania; 
  });

  //funkcja zmiany filtra
  ustawFiltr(nowyFiltr: 'wszystkie' | 'aktywne' | 'zakonczone'){
      this.aktualnyFiltr.set(nowyFiltr);
  }
}
