import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@aspnet/signalr';
import { AuthService } from './auth.service';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  private chatHub: HubConnection;

  constructor(private authService: AuthService,
    private http: HttpClient) {
    this.buildConnection();
  }

  private buildConnection() {
    this.chatHub = new HubConnectionBuilder().withUrl(`${environment.apiUrl}/hubs/chat`, {
      accessTokenFactory: () => this.authService.getToken()
    }).build();
  }

  startConnection() {
    if (!this.chatHub) {
      this.buildConnection();
    }
    return this.chatHub.start();
  }

  sendMessage(message: ChatMessage) {
    this.chatHub.invoke("SendMessage", message);
  }

  setupReceiveMessageHook(callback: (...args: any[]) => void) {
    this.chatHub.on('ReceiveMessage', callback);
  }

  getLast50Messages() {
    return this.http.get<ChatMessage[]>(`${environment.apiUrl}/api/messages`);
  }

  isConnected() {
    return this.chatHub.state == HubConnectionState.Connected;
  }

  reconnect() {
    if (!this.isConnected()) {
      this.startConnection();
    }
  }

  stopConnection() {
    if (this.chatHub) {
      this.chatHub.stop();
      this.chatHub = null;
    }
  }
}


export class ChatMessage {
  message: string;
  userName: string;
  createdAt?: string;
}

