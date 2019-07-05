﻿using BrawlLib.SSBBTypes;
using System;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class TBSTNode : StageTableNode
    {
        public override ResourceType ResourceFileType => ResourceType.TBST;
        internal TBST* Header => (TBST*) WorkingUncompressed.Address;
        internal override string DocumentationSubDirectory => "TBST";
        internal override int EntryOffset => 0x10;

        public TBSTNode(int numEntries)
        {
            unk0 = 1;
            unk1 = 0;
            unk2 = 0;
            while (EntryList.Count < numEntries)
            {
                EntryList.Add(0);
            }
        }

        public override bool OnInitialize()
        {
            unk0 = Header->_unk0;
            unk1 = Header->_unk1;
            unk2 = Header->_unk2;
            for (int i = 0; i < WorkingUncompressed.Length - EntryOffset; i += 4)
            {
                EntryList.Add(((byte*) Header->Entries)[i + 3]);
                EntryList.Add(((byte*) Header->Entries)[i + 2]);
                EntryList.Add(((byte*) Header->Entries)[i + 1]);
                EntryList.Add(((byte*) Header->Entries)[i]);
            }

            return false;
        }

        protected override string GetName()
        {
            return base.GetName("TBST");
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            TBST* header = (TBST*) address;
            header->_tag = TBST.Tag;
            header->_unk0 = unk0;
            header->_unk1 = unk1;
            header->_unk2 = unk2;
            for (int i = 0; i * 4 < EntryList.Count; i++)
            {
                header->Entries[i] = GetFloat(i);
            }
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((TBST*) source.Address)->_tag == TBST.Tag ? new TBSTNode(0) : null;
        }
    }
}