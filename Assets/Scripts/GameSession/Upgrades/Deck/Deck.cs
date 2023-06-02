﻿using System.Collections.Generic;

public class Deck
{
    public string Name { get; set; }
    public int Id { get; set; }
    public string Description { get; set; }
    public CardTag Tags { get; set; }
    public Stack<int> CardsId { get; set; }
}