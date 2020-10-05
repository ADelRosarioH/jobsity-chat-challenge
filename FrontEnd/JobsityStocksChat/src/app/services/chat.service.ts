import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { AuthService } from './auth.service';

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
    this.chatHub = new HubConnectionBuilder().withUrl("/hubs/chat", {
      accessTokenFactory: () => this.authService.getToken()
    }).build();
  }

  startConnection() {
    return this.chatHub.start();
  }

  sendMessage(message: ChatMessage) {
    this.chatHub.invoke("SendMessage", message);
  }

  setupRecieveMessageHook(callback: (...args: any[]) => void) {
    this.chatHub.on('RecieveMessage', callback);
  }

  getLast50Messages() {
    return this.http.get<ChatMessage[]>('/api/messages');
  }

}


export class ChatMessage {
  message: string;
  userName: string;
  createdAt?: string;
}

