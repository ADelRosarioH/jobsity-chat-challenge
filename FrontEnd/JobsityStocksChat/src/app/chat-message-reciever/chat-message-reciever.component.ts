import { Component, Input, OnInit } from '@angular/core';
import { ChatMessage } from '../services/chat.service';

@Component({
  selector: 'app-chat-message-reciever',
  templateUrl: './chat-message-reciever.component.html',
  styleUrls: ['./chat-message-reciever.component.css']
})
export class ChatMessageRecieverComponent implements OnInit {
  @Input()
  public message: ChatMessage;

  constructor() { }

  ngOnInit(): void {
  }

}
