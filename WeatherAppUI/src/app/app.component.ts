import { HttpClient } from '@angular/common/http';
import { IWeatherResponse } from './weatherresponse.interface';
import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Weather App';
  selectedCity: string = '';
  weatherResponse: IWeatherResponse | undefined;

  constructor(private httpClient: HttpClient){
  }

  onChange() {
    if (this.selectedCity === '') {
      return;
    }
    const baseUri = environment.apiUrl;
    this.httpClient.get<IWeatherResponse>(`${baseUri}/Weather?cityName=${this.selectedCity}`)
      .subscribe((data: IWeatherResponse) => {
        this.weatherResponse = data;
      }
    );
  }
}
