import { Pipe, PipeTransform } from '@angular/core';
import { HousingComponent } from './pages/user/housing/housing.component';
import { DetailsComponent } from './pages/user/details/details.component';
import { HomeComponent } from './pages/user/home/home.component';
import { PlacesComponent } from './pages/owner/places/places.component';

@Pipe({
  name: 'search',
  standalone: true
})
export class SearchPipe implements PipeTransform {

  transform(places: HousingComponent[], searchTerm: string): HousingComponent[] {
    if (!searchTerm || searchTerm.trim() === '') {
      return places; 
    }
    searchTerm = searchTerm.toLowerCase();
    return places.filter(place => place.appartment.gender.toLowerCase().includes(searchTerm.toLowerCase()));
  }

}
