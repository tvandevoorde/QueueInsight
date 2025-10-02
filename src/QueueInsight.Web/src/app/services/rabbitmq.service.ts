import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { 
  VirtualHost, 
  Queue, 
  MessageResponse, 
  PublishMessageRequest, 
  DeleteMessageRequest, 
  MoveMessageRequest 
} from '../models/rabbitmq.models';

@Injectable({
  providedIn: 'root'
})
export class RabbitmqService {
  private apiUrl = 'http://localhost:5000/api';

  constructor(private http: HttpClient) { }

  getVirtualHosts(): Observable<VirtualHost[]> {
    return this.http.get<VirtualHost[]>(`${this.apiUrl}/vhosts`);
  }

  getQueues(vhost: string): Observable<Queue[]> {
    return this.http.get<Queue[]>(`${this.apiUrl}/queues/${encodeURIComponent(vhost)}`);
  }

  getMessages(vhost: string, queue: string, count: number = 10): Observable<MessageResponse> {
    return this.http.get<MessageResponse>(
      `${this.apiUrl}/queues/${encodeURIComponent(vhost)}/${encodeURIComponent(queue)}/messages?count=${count}`
    );
  }

  publishMessage(request: PublishMessageRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/messages/publish`, request);
  }

  deleteMessages(request: DeleteMessageRequest): Observable<any> {
    return this.http.delete(`${this.apiUrl}/messages/delete`, { body: request });
  }

  moveMessages(request: MoveMessageRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/messages/move`, request);
  }
}

