
export interface Route {
    routeId: number;
    flightFare: number;
    departureDateTime: string;
    arrivalDateTime: string;
    airlineName: string;
    sourceAirportName: string;
    sourceAirportId: string;
    sourceAirportCity: string;
    sourceAirportCountry: string;
    destinationAirportName: string;
    destinationAirportId: string;
    destinationAirportCity: string;
    destinationAirportCountry: string;
    sourceLat: number;
    sourceLon: number;
    destinationLat: number;
    destinationLon: number;
  }
