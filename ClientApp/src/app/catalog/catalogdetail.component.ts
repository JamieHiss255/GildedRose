import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { CatalogItem } from './catalog.component';

@Component({
  selector: 'app-catalog-component',
  templateUrl: './catalogdetail.component.html'
})
export class CatalogDetailComponent implements OnInit, OnDestroy {
  id: string;
  sub: any;
  itemDetails: CatalogItem;
  qualityDate: Date;


  constructor(private route: ActivatedRoute, private http: HttpClient){
  }

  ngOnDestroy(): void {
  }

  ngOnInit(): void {
    this.sub = this.route.params.subscribe(params => {
      this.id = params['id'];
      this.getItemById().subscribe(item =>{ this.itemDetails = item; });
  });
  }

  getItemById(): Observable<CatalogItem>{
    var url = `api/itemcatalog/${this.id}`;
    return this.http.get<CatalogItem>(url);
  }
}

