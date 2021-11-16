import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-catalog-component',
  templateUrl: './catalog.component.html'
})
export class CatalogComponent implements OnInit {
  allItems: any[];
  catalogItems: any[];
  qualityDate: Date;
  hasAdvancedDay: boolean = false;
  isViewingDegraded: boolean = false;
  catalogUrl: string = 'api/itemcatalog';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private router: Router,
    private http: HttpClient){}

  ngOnInit(): void {
    this.qualityDate = new Date();
    this.getAllItems();
  }

  getAllItems(){
    return this.http.get<CatalogItem[]>(this.catalogUrl + `?currentDate=${this.qualityDate.toISOString()}`)
    .toPromise()
    .then((res) => {
      this.allItems = res; 
      this.catalogItems = res;
      this.isViewingDegraded = false;
    })
    .catch((err) => {
      console.log(err);
    });
  }
  
  viewDetail(id: string){
    this.router.navigate(['/catalogdetail', id])
  }

  advanceDate(){
    var newDate = this.qualityDate;
    newDate.setDate(newDate.getDate() + 1);
    this.qualityDate = new Date(
      newDate.getFullYear(),
      newDate.getMonth() ,
      newDate.getDate()
    );
    this.hasAdvancedDay = true;
    this.getAllItems();
  }

  resetDate(){
    this.qualityDate = new Date();
    this.hasAdvancedDay = false;
    this.getAllItems();
  }

  viewDegraded(){
    this.isViewingDegraded = true;
    this.catalogItems = this.allItems.filter(item => item.quality == 0);
  }

  viewAll(){
    this.isViewingDegraded = false;
    this.catalogItems = this.allItems;
  }

  loadItems(){
    this.http.post<any>(this.catalogUrl, 'file.txt' ).subscribe(data => {
      this.getAllItems();
    })
  }
}

export class CatalogItem{
  id: string;
  name: string;
  category: string;
  sellin: number;
  quality: number;

  constructor(id: string, name:string, category: string, sellin: number, quality: number){
    this.id = id;
    this.name = name;
    this.category = category;
    this.sellin = sellin;
    this.quality = quality;
  }
}

