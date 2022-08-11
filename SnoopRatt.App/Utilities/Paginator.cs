using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Utilities
{
    public abstract class Paginator
    {
        private bool _working;

        public IUserMessage? Message { get; private set; }
        public int Current { get; private set; }
        public abstract int Max { get; }
        public DateTime LastAction { get; private set; } = DateTime.UtcNow;

        protected virtual IEmote LeftEmote => Emoji.Parse("⬅️");
        protected virtual IEmote RightEmote => Emoji.Parse("➡️");

        public async Task Initialize(IUserMessage message)
        {
            _working = false;

            this.Message = message;

            await this.Goto(0);

            await this.Message.AddReactionAsync(this.LeftEmote);
            await this.Message.AddReactionAsync(this.RightEmote);

            return;
        }

        public async Task React(IEmote reaction)
        {
            if(reaction.Equals(this.LeftEmote))
            {
                await this.Prev();
                return;
            }

            if (reaction.Equals(this.RightEmote))
            {
                await this.Next();
                return;
            }
        }

        public async Task Next()
        {
            var next = (this.Current + 1) % this.Max;

            await this.Goto(next);
        }

        public async Task Prev()
        {
            if(this.Current == 0)
            {
                await this.Goto(this.Max - 1);
                return;
            }

            await this.Goto(this.Current - 1);
        }

        public async Task Goto(int page)
        {
            if(_working)
            {
                return;
            }

            if (page > this.Max)
            {
                return;
            }

            if (page < 0)
            {
                return;
            }

            if (this.Message == null)
            {
                return;
            }

            _working = true;

            try
            {
                var attachments = new List<FileAttachment>();
                var embed = new EmbedBuilder();
                embed.WithFooter($"{page + 1}/{this.Max}");

                await this.Load(page, embed, attachments);

                await this.Message.ModifyAsync(mp =>
                {
                    mp.Content = string.Empty;
                    mp.Embed = embed.Build();
                    mp.Attachments = attachments;
                    mp.AllowedMentions = AllowedMentions.None;
                }, new RequestOptions()
                {
                    RetryMode = RetryMode.RetryRatelimit
                });

                foreach (var attachment in attachments)
                {
                    attachment.Dispose();
                }
            }
            catch(Exception e)
            {
                await this.Message.ModifyAsync(mp =>
                {
                    mp.Content = e.Message;
                }, new RequestOptions()
                {
                    RetryMode = RetryMode.RetryRatelimit
                });
            }


            _working = false;

            this.Current = page;

            this.LastAction = DateTime.UtcNow;
        }

        public virtual async Task Idle()
        {
            if(this.Message is null)
            {
                return;
            }

            await this.Message.RemoveAllReactionsAsync();
        }

        public abstract Task Load(int page, EmbedBuilder embed, List<FileAttachment> attachments);
    }
}
