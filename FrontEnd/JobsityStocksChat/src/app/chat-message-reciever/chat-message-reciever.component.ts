import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { ChatMessage } from '../services/chat.service';

@Component({
  selector: 'app-chat-message-reciever',
  templateUrl: './chat-message-reciever.component.html',
  styleUrls: ['./chat-message-reciever.component.css']
})
export class ChatMessageRecieverComponent implements OnInit, AfterViewInit {
  @Input()
  public message: ChatMessage;
  container: HTMLElement;

  constructor() { }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    this.container = document.getElementById("messages-container");
    this.container.scrollTop = this.container.scrollHeight;
  }

}
