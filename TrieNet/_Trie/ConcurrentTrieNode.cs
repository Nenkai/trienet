// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Gma.DataStructures.StringSearch
{
    public class ConcurrentTrieNode<TValue> : TrieNodeBase<TValue>
    {
        private readonly ConcurrentDictionary<char, ConcurrentTrieNode<TValue>> m_Children;
        private readonly ConcurrentQueue<TValue> m_Values;

        public ConcurrentTrieNode()
        {
            m_Children = new ConcurrentDictionary<char, ConcurrentTrieNode<TValue>>();
            m_Values = new ConcurrentQueue<TValue>();
        }


        protected override int KeyLength => 1;

        protected override IEnumerable<TValue> Values()
            => m_Values;

        protected override IEnumerable<TrieNodeBase<TValue>> Children()
            => m_Children.Values;

        protected override void AddValue(TValue value)
            => m_Values.Enqueue(value);

        protected override TrieNodeBase<TValue> GetOrCreateChild(char key)
            => m_Children.GetOrAdd(key, new ConcurrentTrieNode<TValue>());

        protected override TrieNodeBase<TValue> GetChildOrNull(string query, int position)
        {
            if (query == null) 
                throw new ArgumentNullException("query");

            return m_Children.TryGetValue(query[position], out ConcurrentTrieNode<TValue> childNode)
                    ? childNode
                    : null;
        }
    }
}