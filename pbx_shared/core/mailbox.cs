namespace pbx_shared
{
    public class mailbox
    {
        // 2+ digits, unique
        public string mailbox_nbr { get; set; }

        public string name { get; set; }

        public string password { get; set; }

        public string email { get; set; }
        public bool skip_voicemail_instructions { get; set; }
        public bool hide_mailbox_From_directory { get; set; }
        public enum voicemail_delete_timeframes { never, week, month, year}

        public voicemail_delete_timeframes voicemail_delete_timeframe = voicemail_delete_timeframes.never;

        public string timezone { get; set; }

        public enum greeting_types { none, wav }
        public string greeting_file { get; set; }

    }
}
