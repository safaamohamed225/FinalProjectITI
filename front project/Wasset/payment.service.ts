import { HttpClient } from '@angular/common/http';
import { Injectable, Input, input } from '@angular/core';
import { Observable } from 'rxjs';
import { PlacesOwnerService } from './src/app/services/places-owner.service';

@Injectable({
  providedIn: 'root',
})
export class PaymentService {
@Input()p:number=0;
apartmentPrice:number=0;
constructor(private _place:PlacesOwnerService) {
  console.log(this._place.price)
  this.apartmentPrice=this._place.price;
}

  Api: string =
    'ZXlKaGJHY2lPaUpJVXpVeE1pSXNJblI1Y0NJNklrcFhWQ0o5LmV5SmpiR0Z6Y3lJNklrMWxjbU5vWVc1MElpd2ljSEp2Wm1sc1pWOXdheUk2T1RZNU1Ua3hMQ0p1WVcxbElqb2lhVzVwZEdsaGJDSjkueUp5UGhWS0dyUE5obGZmVVZhLWs1RjBJRXdiNlk5T01MNXFOdzExTWwxLWowQ1JsU2JYSWxSNkFUS2VGWVB4R2MzLU1XXzd2cWtwZkVvUVlSQVNqOFE=';

  CardRequest = async (): Promise<void> => {
    let data1: { api_key: string } = {
      api_key: this.Api,
    };
    //Send The Request
    let sendRequest = await fetch('https://accept.paymob.com/api/auth/tokens', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data1),
    });
    let response = await sendRequest.json();
    let token: string = response.token;
    this.sendToken(token);
  };

  //Start Step Two ==> Send The Token
  // i Have to call The first Func ==>  Think Wrong
  sendToken = async (token: string): Promise<void> => {
    let data2: {
      auth_token: string;
      delivery_needed: string;
      amount_cents: string;
      items: any[];
    } = {
      auth_token: token,
      delivery_needed: 'false',
      amount_cents: '5000',
      items: [],
    };
    let sendData = fetch('https://accept.paymob.com/api/ecommerce/orders', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data2),
    });
    let send = await (await sendData).json();
    //Get id i will need it in the next step
    let id: string = send.id;
    this.endAuthentication(id, token);
  };

  endAuthentication = async (id: string, tokenPath: string): Promise<void> => {
    let data3: {
      auth_token: string;
      amount_cents: number;
      expiration: number;
      order_id: string;
      billing_data: {
        apartment: string;
        email: string;
        floor: string;
        first_name: string;
        street: string;
        building: string;
        phone_number: string;
        shipping_method: string;
        postal_code: string;
        city: string;
        country: string;
        last_name: string;
        state: string;
      };
      currency: string;
      integration_id: number;
    } = {
      auth_token: tokenPath,
      amount_cents: this._place.price*100,
      expiration: 3600,
      order_id: id,
      billing_data: {
        apartment: '803',
        email: 'claudette09@exa.com',
        floor: '42',
        first_name: 'Clifford Nicolas',
        street: 'Ethan Land',
        building: '8028',
        phone_number: '+86(8)9135210487',
        shipping_method: 'PKG',
        postal_code: '01898',
        city: 'Jaskolskiburgh',
        country: 'CR',
        last_name: 'Nicolas',
        state: 'Utah',
      },
      currency: 'EGP',
      integration_id: 4550777,
    };
    let sendData = fetch(
      'https://accept.paymob.com/api/acceptance/payment_keys',
      {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data3),
      }
    );
    let readData = await (await sendData).json();
    let lastToken: string = readData.token;
    console.log(lastToken);
    this.payWithCard(lastToken);
  };

  payWithCard = async (lastToken: string): Promise<void> => {
    let iframURl: string = `https://accept.paymob.com/api/acceptance/iframes/836119?payment_token=${lastToken}`;
    location.href = iframURl;
  };

  // First Step , I will Go To create Request To Make Authntication And get Token
  //Get Api Key From The Site
  VodafoneApi: string =
    'ZXlKaGJHY2lPaUpJVXpVeE1pSXNJblI1Y0NJNklrcFhWQ0o5LmV5SmpiR0Z6Y3lJNklrMWxjbU5vWVc1MElpd2ljSEp2Wm1sc1pWOXdheUk2T1RZNU1Ua3hMQ0p1WVcxbElqb2lhVzVwZEdsaGJDSjkueUp5UGhWS0dyUE5obGZmVVZhLWs1RjBJRXdiNlk5T01MNXFOdzExTWwxLWowQ1JsU2JYSWxSNkFUS2VGWVB4R2MzLU1XXzd2cWtwZkVvUVlSQVNqOFE=';

  VodafonerRequest = async (): Promise<void> => {
    let data1 = {
      api_key: this.VodafoneApi,
    };
    //Send The Request
    let sendRequest = await fetch('https://accept.paymob.com/api/auth/tokens', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data1),
    });
    let response = await sendRequest.json();
    let token = response.token;
    this.sendVodafoneToken(token);
  };

  sendVodafoneToken = async (token: string): Promise<void> => {
    let data2 = {
      auth_token: token,
      delivery_needed: 'false',
      amount_cents: '100',
      items: [],
    };
    let sendData = fetch('https://accept.paymob.com/api/ecommerce/orders', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data2),
    });
    let send = await (await sendData).json();
    // console.log(send)
    //Get id i will need it in the next step
    let id = send.id;
    console.log(id);
    console.log(this._place.price)
    this.endVodafoneAuthntication(id, token);
  };

  endVodafoneAuthntication = async (
    id: string,
    tokenPath: string
  ): Promise<void> => {
    let data3 = {
      auth_token: tokenPath,
      amount_cents: this._place.price*100,
      expiration: 3600,
      order_id: id,
      billing_data: {
        apartment: '803',
        email: 'claudette09@exa.com',
        floor: '42',
        first_name: 'Clifford',
        street: 'Ethan Land',
        building: '8028',
        phone_number: '01147056186',
        shipping_method: 'PKG',
        postal_code: '01898',
        city: 'Jaskolskiburgh',
        country: 'CR',
        last_name: 'Nicolas',
        state: 'Utah',
      },
      currency: 'EGP',
      integration_id: 4550807,
    };
    let sendData = fetch(
      'https://accept.paymob.com/api/acceptance/payment_keys',
      {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data3),
      }
    );
    let readData = await (await sendData).json();
    let lastToken = readData.token;
    this.vodafoneCashIntegration(lastToken);
  };

  vodafoneCashIntegration = async (lastToken: string): Promise<void> => {
    let data4 = {
      source: {
        identifier: '01147056186',
        subtype: 'WALLET',
      },
      payment_token: lastToken,
    };
    let sendDataVodafon = fetch(
      'https://accept.paymob.com/api/acceptance/payments/pay',
      {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data4),
      }
    );
    console.log(sendDataVodafon);
    let vodafonData = await (await sendDataVodafon).json();
    console.log(vodafonData);
  };
}
