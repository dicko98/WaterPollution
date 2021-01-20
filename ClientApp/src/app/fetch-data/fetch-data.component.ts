import { AfterViewInit, Component, Inject, ViewChild } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Timestamp } from 'rxjs';
import { Offset } from 'popper.js';
import { MatPaginator, MatTableDataSource, ShowOnDirtyErrorStateMatcher } from '@angular/material';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent implements AfterViewInit{
  public pollutions: Pollution[] = [];
  dataSource = new MatTableDataSource<Pollution>(this.pollutions);
  displayedColumns:  string[] = ['id', 'data', 'time', 'deletion'];
  httpClient: HttpClient;
  baseUrl: string;
  public sensors: string[];
  selectedValue: string;

  @ViewChild(MatPaginator, {static: false}) paginator: MatPaginator;
  

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Pollution[]>(baseUrl + 'pollution').subscribe(result => {
      this.dataSource = new MatTableDataSource<Pollution>(result);
      this.ngAfterViewInit();
    }, error => console.error(error));
    this.httpClient = http;
    this.baseUrl = baseUrl;
    http.get<string[]>(baseUrl + 'pollution/getsensors').subscribe(result => {
      this.sensors = result;
    }, error => console.error(error));
  }

  selected(event) {
    this.selectedValue = event.value;
  }
  

  deleteData(pollution, event){
    const options = {
      headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }), body: pollution
    };

   this.httpClient.delete(this.baseUrl + 'pollution/deletebykey', options)
     .subscribe((s) => {
      console.log(s);
      location.reload();
    });
  }

  deleteSensorData(){
    this.httpClient.delete(this.baseUrl + 'pollution/' + this.selectedValue)
    .subscribe((s) => {
     console.log(s);
     location.reload();
   });
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }
}

interface Pollution {
  sensorId: string;
  sensorData: number;
  collectTime: Date;
}
