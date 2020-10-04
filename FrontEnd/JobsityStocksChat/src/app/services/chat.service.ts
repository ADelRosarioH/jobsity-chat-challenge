import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  private chatHub: HubConnection;

  constructor() {
    this.buildConnection();
  }

  private buildConnection() {
    this.chatHub = new HubConnectionBuilder().withUrl("http://localhost:5000/hubs/chat").build();
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

}


export class ChatMessage {
  message: string;
  userName: string;
  date: string
}

